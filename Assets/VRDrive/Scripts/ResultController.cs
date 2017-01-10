using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Linq;
using System;

/// Class for controlling result
public class ResultController : MonoBehaviour {

	public static ResultController instance;

	private Dictionary<string, UserState> playerStateList = new Dictionary<string, UserState>();

	private GameObject[] checkListBoxes;
	private GameObject pickUp;
	private GameObject evaluation;
	private GameObject preview;
	private GameObject total;

	private List<string> checkTextList = new List<string>();

	private int selectBox = 0;
	private int page = 0;
	private int player = 0;

	private Dictionary<string, string> checkTextJPWordList = new Dictionary<string, string>() {
		{"AI", "他車"},
		{"Stop", "止まれ"},
		{"Signal", "信号処理"},
		{"kmh", "速度制限"}
	};

	private Dictionary<string, string> checkTextJPEvaluationList = new Dictionary<string, string>() {
		{"AI", "他の車に注意しながら走行しましょう"},
		{"Stop", "止まれの標識を見逃さないようにしましょう"},
		{"Signal", "信号に注意して走行しましょう"},
		{"kmh", "速度を守りましょう"}
	};

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
		SetGameObjectForInitialization ();

		if (GameObject.Find ("ValueKeeper")) {
			GameObject valueKeeper = GameObject.Find ("ValueKeeper").gameObject;
			Dictionary<string, UserState> userStateList = valueKeeper.GetComponent<ValueKeeper> ().UserStateList;

			foreach (KeyValuePair<string, UserState> eachUserState in userStateList) {
				if (eachUserState.Key.Contains ("Player")) {
					playerStateList.Add (eachUserState.Key, eachUserState.Value);
				}
			}

			if (playerStateList.Count > 0) {
				Dictionary<string, bool> checks = playerStateList.First().Value.CheckList;
				foreach (string check in checks.Keys) {
					checkTextList.Add (check);
				}
			}
			Destroy (valueKeeper);

			PickUpCheckBox ();
			ChangeContentsOfCheckBoxes ();
		}

		SoundController.instance.StartResultSound();
	}

	private void FixedUpdate() {
		float h = Input.GetAxis("Handle");
		if (h == 0) {
			h = CrossPlatformInputManager.GetAxis("Horizontal");
		}
		bool d = CrossPlatformInputManager.GetButtonUp("Decide");
		StartCoroutine(ChangeSelectedCheck(h, d, flag));
	}

	void SetGameObjectForInitialization() {
		checkListBoxes = GameObject.FindGameObjectsWithTag ("Check").OrderBy(checkListBox => checkListBox.name).ToArray();
		Transform canvasTransform = GameObject.Find ("Canvas").gameObject.transform;
		pickUp = canvasTransform.FindChild ("PickUp").gameObject;
		evaluation = canvasTransform.FindChild ("Evaluation").gameObject;
		preview = canvasTransform.FindChild ("Preview").gameObject;
		total = canvasTransform.FindChild ("Total").gameObject;
	}

	private string ConvertWordForJPWordList(string key) {
		if(key.Contains("kmh")) {
			return "kmh";
		}
		return key;
	}

	/// <summary>Change selected check.</summary>
	/// <param name="horizontal">The length of the delay</param>
	/// <param name="decide">Go to the menu scene or not</param>
	/// <param name="changingNow">Whether already selected the check or not</param>
	private IEnumerator ChangeSelectedCheck(float horizontal, bool decide, bool changingNow) {
		if(!changingNow) {
			if(decide) {
				flag = true;

				SoundController.instance.ShotClipSound("decide");
				yield return new WaitForSeconds(SoundController.instance.GetClipLength("decide"));

				// TODO if multiple user and if push decode, change the user 
				SceneManager.LoadScene("menu");
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
				MoveCheckBoxes (move);

				yield return new WaitForSeconds(delay);
				flag = false;
			}
		}
	}

	private void MoveCheckBoxes(int move) {
		if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(
				checkListBoxes[selectBox % checkListBoxes.Length].GetComponent<Text>(), null, fontColor
			);
		}

		selectBox += move;

		if (selectBox < 0 || selectBox > checkTextList.Count - 1) {
			selectBox -= move;
		}

		int newPage = Mathf.FloorToInt((float)selectBox / (float)checkListBoxes.Length);
		if (page != newPage) {
			page = newPage;
			ChangeContentsOfCheckBoxes ();
		}

		if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(
				checkListBoxes[selectBox % checkListBoxes.Length].GetComponent<Text>(), null, fontColor
			);
		}

		PickUpCheckBox ();
		SoundController.instance.ShotClipSound("select");
	}

	private void PickUpCheckBox() {
		string target = ConvertWordForJPWordList(checkTextList [selectBox]);
		pickUp.GetComponent<Text> ().text = checkTextJPWordList[target];
		evaluation.GetComponent<Text> ().text = checkTextJPEvaluationList[target];
		// Preview画像のloop開始
	}

	private IEnumerator ShowPreview(float interval, string checkName, string userName) {
		// summary : checktextlist[selectBox]フォルダにあるPlayerNo-****の画像を連番で呼び出す

		// playerStateList ["Player" + player].CheckList [checkTextList [selectBox]]がtrue = clearしてるので画像は一つだけ
		// falseならmissしてる = summary : checktextlist[selectBox]フォルダにあるPlayerNo-****の画像を連番で呼び出す
		// whileループを抜ける条件は次の項目をセレクトしたら
		while (true) {
			yield return new WaitForSeconds(interval);
		}
	}

	private void ChangeContentsOfCheckBoxes() {
		int count = 0;
		int value;
		RawImage ImageOk;
		RawImage ImageNg;
		foreach (GameObject checkListBox in checkListBoxes) {
			value = page * checkListBoxes.Length + count;
			ImageOk = checkListBox.transform.FindChild ("Ok").gameObject.GetComponent<RawImage> ();
			ImageNg = checkListBox.transform.FindChild ("Ng").gameObject.GetComponent<RawImage> ();
			if (value < checkTextList.Count) {
				checkListBox.GetComponent<Text> ().text = checkTextJPWordList [ConvertWordForJPWordList (checkTextList [value])];
				if (playerStateList ["Player" + player].CheckList [checkTextList [value]]) {
					ViewerController.instance.ChangeRawImageState (ImageOk, true);
					ViewerController.instance.ChangeRawImageState (ImageNg, false);
				} else {
					ViewerController.instance.ChangeRawImageState (ImageOk, false);
					ViewerController.instance.ChangeRawImageState (ImageNg, true);
				}
			} else {
				checkListBox.GetComponent<Text> ().text = "";
				ViewerController.instance.ChangeRawImageState (ImageOk, false);
				ViewerController.instance.ChangeRawImageState (ImageNg, false);
			}
			count++;
		}
	}

}
