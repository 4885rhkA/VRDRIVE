using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Linq;

public class MenuController : MonoBehaviour {

	public static MenuController instance;

	private GameObject[] stageObjects;
	private int selectStageNo = 0;

	private Color fontColor = new Color();
	private string colorSelected = "#FFFFFFFF";
	private string colorNoSelected = "#646464FF";

	private bool changingSelectionFlag = false;
	private float timeForChangingSelection = 0.25f;

	void Awake() {
		instance = this;
	}

	void Start() {
		stageObjects = GameObject.FindGameObjectsWithTag("StageName");
		if(stageObjects != null) {
			stageObjects = stageObjects.OrderBy(stageObject => stageObject.name).ToArray();
		}
		stageObjects[0].GetComponent<RawImage>().enabled = true;
		if(ColorUtility.TryParseHtmlString(colorSelected, out fontColor)) {
			stageObjects[0].transform.FindChild(stageObjects[0].name + "Text").GetComponent<Text>().color = fontColor;
		}
		else {
			Debug.LogWarning("The color" + colorSelected + "cannnot convert into Color class.");
		}
	}

	private void FixedUpdate() {
		float h = Input.GetAxis("Handle");
		if (h == 0) {
			h = CrossPlatformInputManager.GetAxis("Horizontal");
		}
		bool b = Input.GetKey(KeyCode.E);
		StartCoroutine(ChangeStage(h, b, changingSelectionFlag));
	}

	private IEnumerator ChangeStage(float horizontal, bool button, bool isSelectionChanging) {
		if(!isSelectionChanging) {
			if(button) {
				changingSelectionFlag = true;
				SceneManager.LoadScene(stageObjects[selectStageNo].name.Substring(2));
			}
			else if(Mathf.Abs(horizontal) > 0.5) {
				changingSelectionFlag = true;

				stageObjects[selectStageNo].GetComponent<RawImage>().enabled = false;
				if(ColorUtility.TryParseHtmlString(colorNoSelected, out fontColor)) {
					stageObjects[selectStageNo].transform.FindChild(stageObjects[selectStageNo].name + "Text").GetComponent<Text>().color = fontColor;
				}
				else {
					Debug.LogWarning("The color" + colorSelected + "cannnot convert into Color class.");
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

				stageObjects[selectStageNo].GetComponent<RawImage>().enabled = true;
				if(ColorUtility.TryParseHtmlString(colorSelected, out fontColor)) {
					stageObjects[selectStageNo].transform.FindChild(stageObjects[selectStageNo].name + "Text").GetComponent<Text>().color = fontColor;
				}
				else {
					Debug.LogWarning("The color" + colorSelected + "cannnot convert into Color class.");
				}

				yield return new WaitForSeconds(timeForChangingSelection);
				changingSelectionFlag = false;
			}
		}
	}

}
