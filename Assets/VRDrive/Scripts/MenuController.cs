using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Linq;

/// Class for controlling menu
public class MenuController : MonoBehaviour {

	public static MenuController instance;

	private GameObject[] sceneObjects;
	private int sceneNo = 0;

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{"selected", "#FFFFFFFF"},
		{"noSelected", "#646464FF"}
	};

	private bool flag = false;
	private float delay = 0.1f;

	void Awake() {
		instance = this;
	}

	void Start() {
		sceneObjects = GameObject.FindGameObjectsWithTag("Scene");

		if(sceneObjects != null) {
			sceneObjects = sceneObjects.OrderBy(sceneObject => sceneObject.name).ToArray();
		}

		MoveSelection (0);

		SoundController.instance.StartMenuSound();
	}

	private void FixedUpdate() {
		float h = Input.GetAxis("Handle");
		if (h == 0) {
			h = CrossPlatformInputManager.GetAxis("Horizontal");
		}
		bool d = CrossPlatformInputManager.GetButtonUp("Decide");
		StartCoroutine(ChangeSelectedScene(h, d, flag));
	}

	/// <summary>Change selected scene.</summary>
	/// <param name="horizontal">The length of the delay</param>
	/// <param name="decide">Go to the selected scene or not</param>
	/// <param name="changingNow">Whether already selected the scene or not</param>
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

				MoveSelection (move);
				SoundController.instance.ShotClipSound("select");

				yield return new WaitForSeconds(delay);
				flag = false;
			}
		}
	}

	/// <summary>Moves the selection.</summary>
	/// <param name="move">Param for move up or down</param>
	private void MoveSelection(int move) {
		if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(
				sceneObjects[sceneNo].transform.FindChild(sceneObjects[sceneNo].name + "Text").GetComponent<Text>(), null, fontColor
			);
		}
		ViewerController.instance.ChangeRawImageState(sceneObjects[sceneNo].GetComponent<RawImage>(), false);

		sceneNo += move;

		if (sceneNo < 0 || sceneNo > sceneObjects.Length - 1) {
			sceneNo -= move;
		}

		if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(
				sceneObjects[sceneNo].transform.FindChild(sceneObjects[sceneNo].name + "Text").GetComponent<Text>(), null, fontColor
			);
		}
		ViewerController.instance.ChangeRawImageState(sceneObjects[sceneNo].GetComponent<RawImage>(), true);
	}

}
