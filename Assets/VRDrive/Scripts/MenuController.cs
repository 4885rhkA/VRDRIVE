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

	private GameObject[] stageObjects;
	private int selectStageNo = 0;

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{"selected", "#FFFFFFFF"},
		{"noSelected", "#646464FF"}
	};

	private bool changingSelectionFlag = false;
	private float timeForChangingSelection = 0.1f;

	void Awake() {
		instance = this;
	}

	void Start() {
		stageObjects = GameObject.FindGameObjectsWithTag("StageName");
		if(stageObjects != null) {
			stageObjects = stageObjects.OrderBy(stageObject => stageObject.name).ToArray();
		}
		if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(
				stageObjects[0].transform.FindChild(stageObjects[0].name + "Text").GetComponent<Text>(), null, fontColor
			);
		}
		ViewerController.instance.ChangeRawImageState(stageObjects[0].GetComponent<RawImage>(), true);
		SoundController.instance.StartMenuSound();
	}

	private void FixedUpdate() {
		float h = Input.GetAxis("Handle");
		if (h == 0) {
			h = CrossPlatformInputManager.GetAxis("Horizontal");
		}
		bool d = CrossPlatformInputManager.GetButtonUp("Decide");
		StartCoroutine(ChangeSelectedStage(h, d, changingSelectionFlag));
	}

	/// <summary>Change selected stage.</summary>
	/// <param name="horizontal">The length of the delay</param>
	/// <param name="decideStage">Go to the selected stage or not</param>
	/// <param name="isSelectionChanging">Whether already selected the stage or not</param>
	private IEnumerator ChangeSelectedStage(float horizontal, bool decideStage, bool isSelectionChanging) {
		if(!isSelectionChanging) {
			if(decideStage) {
				changingSelectionFlag = true;
				SoundController.instance.ShotClipSound("decide");
				yield return new WaitForSeconds(SoundController.instance.GetClipLength("decide"));
				SceneManager.LoadScene(stageObjects[selectStageNo].name.Substring(2));
			}
			else if(Mathf.Abs(horizontal) > 0.5) {
				changingSelectionFlag = true;
				stageObjects[selectStageNo].GetComponent<RawImage>().enabled = false;
				if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
					GameObject stageObject = stageObjects[selectStageNo];
					stageObject.transform.FindChild(stageObject.name + "Text").GetComponent<Text>().color = fontColor;
				}
				else {
					Debug.LogWarning("The color" + colorList["selected"] + "cannnot convert into Color class.");
				}

				if(horizontal > 0) {
					selectStageNo++;
				}
				else {
					selectStageNo--;
				}
				if(selectStageNo > stageObjects.Length - 1 || selectStageNo < 0) {
					selectStageNo = stageObjects.Length - Mathf.Abs(selectStageNo);
				}

				if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
					GameObject stageObject = stageObjects[selectStageNo];
					ViewerController.instance.ChangeTextContent(
						stageObject.transform.FindChild(stageObject.name + "Text").GetComponent<Text>(), null, fontColor
					);
				}
				ViewerController.instance.ChangeRawImageState(stageObjects[selectStageNo].GetComponent<RawImage>(), true);
				SoundController.instance.ShotClipSound("select");
				yield return new WaitForSeconds(timeForChangingSelection);
				changingSelectionFlag = false;
			}
		}
	}

}
