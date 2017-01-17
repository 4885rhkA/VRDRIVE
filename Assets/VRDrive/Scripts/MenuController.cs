using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.VR;

/// <summary>
/// Menu controller.
/// </summary>
public class MenuController : MonoBehaviour {

	public static MenuController instance;

	[SerializeField] private bool keyboardMode = false;
	[SerializeField] private bool handleMode = true;

	private GameObject[] sceneObjects;
	private int sceneNo = 0;

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{ "selected", "#FFFFFFFF" },
		{ "noSelected", "#646464FF" }
	};

	private bool flag = false;
	private float delay = 1f;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		sceneObjects = GameObject.FindGameObjectsWithTag("Scene");

		if(sceneObjects != null) {
			sceneObjects = sceneObjects.OrderBy(sceneObject => sceneObject.name).ToArray();
		}

		MoveSelection(0);

		SoundController.instance.StartMenuSound();
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate() {
		float h = 0;
		if(keyboardMode) {
			h = CrossPlatformInputManager.GetAxis("Horizontal");
		}
		else if(handleMode) {
			h = Input.GetAxis("Handle");
		}
		bool d = CrossPlatformInputManager.GetButtonUp("Decide");
        if (CrossPlatformInputManager.GetButton("Reset"))
        {
            InputTracking.Recenter();
        }
        StartCoroutine(ChangeSelectedScene(h, d, flag));
	}

	/// <summary>
	/// Changes the selected scene.
	/// </summary>
	/// <returns>The selected scene.</returns>
	/// <param name="horizontal">Horizontal.</param>
	/// <param name="decide">If set to <c>true</c> decide.</param>
	/// <param name="changingNow">If set to <c>true</c> changing now.</param>
	private IEnumerator ChangeSelectedScene(float horizontal, bool decide, bool changingNow) {
		if(!changingNow) {
			if(decide) {
				flag = true;
				SoundController.instance.ShotClipSound("decide");
				yield return new WaitForSeconds(SoundController.instance.GetClipLength("decide"));
				SceneManager.LoadScene(sceneObjects[sceneNo].name.Substring(2));
			}
			else if(Mathf.Abs(horizontal) > 0.5) {
				flag = true;

				int move = 0;
				if(horizontal > 0) {
					move = 1;
				}
				else {
					move = -1;
				}

				MoveSelection(move);

				yield return new WaitForSeconds(delay);
				flag = false;
			}
		}
	}

	/// <summary>
	/// Moves the selection.
	/// </summary>
	/// <param name="move">Move.</param>
	private void MoveSelection(int move) {
		if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(sceneObjects[sceneNo].transform.FindChild(sceneObjects[sceneNo].name + "Text").GetComponent<Text>(), null, fontColor);
		}
		ViewerController.instance.ChangeRawImageState(sceneObjects[sceneNo].GetComponent<RawImage>(), false);

		sceneNo += move;

		if(sceneNo < 0 || sceneNo > sceneObjects.Length - 1) {
			sceneNo -= move;
		}
		else {
			SoundController.instance.ShotClipSound("select");
		}

		if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(sceneObjects[sceneNo].transform.FindChild(sceneObjects[sceneNo].name + "Text").GetComponent<Text>(), null, fontColor);
		}
		ViewerController.instance.ChangeRawImageState(sceneObjects[sceneNo].GetComponent<RawImage>(), true);
	}

}
