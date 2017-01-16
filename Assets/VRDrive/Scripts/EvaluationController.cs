using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

/// <summary>
/// Evaluation controller.
/// </summary>
public class EvaluationController : MonoBehaviour {

	public static EvaluationController instance;

	[SerializeField] private bool keyboardMode = false;
	[SerializeField] private bool handleMode = true;

	[SerializeField] private bool japaneseMode = true;

	[SerializeField] private Texture2D successTexture = null;
	[SerializeField] private Texture2D noPreviewTexture = null;

	private Dictionary<string, UserState> playerStateList = new Dictionary<string, UserState>();
	private Dictionary<string, List<Texture2D>> playerScreenshotList;

	private GameObject[] checkListBoxes;
	private GameObject pickUp;
	private GameObject evaluation;
	private GameObject preview;
	private GameObject total;

	private int selectBox = 0;
	private int page = -1;
	private int player = 0;

	private List<string> checkTextList = new List<string>();

	private Dictionary<string, string> checkTextJPWordList = new Dictionary<string, string>() {
		{ "AI", "他車" },
		{ "Stop", "止まれ" },
		{ "Signal", "信号処理" },
		{ "kmh", "速度制限" }
	};

	private Dictionary<string, string> checkTextEvaluationList = new Dictionary<string, string>() {
		{ "AI", "Take care of other car" },
		{ "Stop", "Don't go through" },
		{ "Signal", "Look at the signal" },
		{ "kmh", "Don't drive too fast" }
	};

	private Dictionary<string, string> checkTextJPEvaluationList = new Dictionary<string, string>() {
		{ "AI", "他の車に注意しながら走行しましょう" },
		{ "Stop", "止まれの標識を見逃さないようにしましょう" },
		{ "Signal", "信号に注意して走行しましょう" },
		{ "kmh", "速度を守りましょう" }
	};

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{ "selected", "#FFFFFFFF" },
		{ "noSelected", "#646464FF" }
	};

	private bool change = false;
	private float delay = 1f;
	private float interval = 0.5f;

	private string nextScene = "menu";

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
		SetGameObjectForInitialization();

		if(GameObject.Find("ValueKeeper")) {
			GameObject valueKeeperObject = GameObject.Find("ValueKeeper").gameObject;
			ValueKeeper valueKeeper = valueKeeperObject.GetComponent<ValueKeeper>();

			Dictionary<string, UserState> userStateList = valueKeeper.UserStateList;
			foreach(KeyValuePair<string, UserState> eachUserState in userStateList) {
				if(eachUserState.Key.Contains("Player")) {
					playerStateList.Add(eachUserState.Key, eachUserState.Value);
				}
			}
			playerScreenshotList = valueKeeper.PlayerScreenshotList;

			if(playerStateList.Count > 0) {
				Dictionary<string, bool> checks = playerStateList.First().Value.CheckList;
				foreach(string check in checks.Keys) {
					checkTextList.Add(check);
				}
			}

			nextScene = valueKeeper.SceneAfterEvaluation;

			Destroy(valueKeeperObject);

			total.GetComponent<Text>().text = GetTotalComment();

			MoveCheckBoxes(-1);
		}

		SoundController.instance.StartEvaluationSound();
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
		StartCoroutine(ChangeSelectedCheck(h, d, change));
	}

	/// <summary>
	/// Gets the total comment.
	/// </summary>
	/// <returns>The total comment.</returns>
	private string GetTotalComment() {
		int score = GetTotalScore();
		int total = checkTextList.Count();
		return GetPlayerName(player) + "'s Score :" + score.ToString() + "/" + total.ToString();
	}

	/// <summary>
	/// Gets the name of the player.
	/// </summary>
	/// <returns>The player name.</returns>
	/// <param name="number">Number.</param>
	private string GetPlayerName(int number) {
		return "Player" + number;
	}

	/// <summary>
	/// Gets the total score.
	/// </summary>
	/// <returns>The total score.</returns>
	private int GetTotalScore() {
		int score = 0;
		foreach(KeyValuePair<string, bool> check in playerStateList [GetPlayerName (player)].CheckList) {
			if(check.Value) {
				score++;
			}
		}
		return score;
	}

	/// <summary>
	/// Sets the game object for initialization.
	/// </summary>
	private void SetGameObjectForInitialization() {
		checkListBoxes = GameObject.FindGameObjectsWithTag("Check").OrderBy(checkListBox => checkListBox.name).ToArray();
		Transform canvasTransform = GameObject.Find("Canvas").gameObject.transform;
		pickUp = canvasTransform.FindChild("PickUp").gameObject;
		evaluation = canvasTransform.FindChild("Evaluation").gameObject;
		preview = canvasTransform.FindChild("Preview").gameObject;
		total = canvasTransform.FindChild("Total").gameObject;
	}

	/// <summary>
	/// Converts the word.
	/// </summary>
	/// <returns>The word.</returns>
	/// <param name="key">Key.</param>
	private string ConvertWord(string key) {
		if(key.Contains("kmh")) {
			return "kmh";
		}
		return key;
	}

	/// <summary>
	/// Changes the selected check.
	/// </summary>
	/// <returns>The selected check.</returns>
	/// <param name="horizontal">Horizontal.</param>
	/// <param name="decide">If set to <c>true</c> decide.</param>
	/// <param name="changingNow">If set to <c>true</c> changing now.</param>
	private IEnumerator ChangeSelectedCheck(float horizontal, bool decide, bool changingNow) {
		if(!changingNow) {
			if(decide) {
				change = true;
				SoundController.instance.ShotClipSound("decide");
				yield return new WaitForSeconds(SoundController.instance.GetClipLength("decide"));

				player++;
				// If multiple user, show the rvaluation
				if(player < playerStateList.Count) {
					if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
						ViewerController.instance.ChangeTextContent(checkListBoxes[selectBox % checkListBoxes.Length].GetComponent<Text>(), null, fontColor);
					}

					page = -1;
					selectBox = 0;

					MoveCheckBoxes(-1);
					total.GetComponent<Text>().text = GetTotalComment();

					yield return new WaitForSeconds(delay);
					change = false;
				}
				else {
					SceneManager.LoadScene(nextScene);
				}
			}
			else if(Mathf.Abs(horizontal) > 0.5) {
				change = true;
				int move = 0;
				if(horizontal > 0) {
					move = 1;
				}
				else if(horizontal < 0) {
					move = -1;
				}

				MoveCheckBoxes(move);

				yield return new WaitForSeconds(delay);
				change = false;
			}
		}
	}

	/// <summary>
	/// Moves the check boxes.
	/// </summary>
	/// <param name="move">Move.</param>
	private void MoveCheckBoxes(int move) {
		if(ColorUtility.TryParseHtmlString(colorList["noSelected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(checkListBoxes[selectBox % checkListBoxes.Length].GetComponent<Text>(), null, fontColor);
		}

		selectBox += move;

		if(selectBox < 0 || selectBox > checkTextList.Count - 1) {
			selectBox -= move;
		}
		else {
			SoundController.instance.ShotClipSound("select");
		}

		int newPage = Mathf.FloorToInt((float)selectBox / (float)checkListBoxes.Length);
		if(page != newPage) {
			page = newPage;
			ChangeContentsOfCheckBoxes();
		}

		if(ColorUtility.TryParseHtmlString(colorList["selected"], out fontColor)) {
			ViewerController.instance.ChangeTextContent(checkListBoxes[selectBox % checkListBoxes.Length].GetComponent<Text>(), null, fontColor);
		}

		PickUpCheckBox();
	}

	/// <summary>
	/// Changes the contents of check boxes.
	/// </summary>
	private void ChangeContentsOfCheckBoxes() {
		int count = 0;
		int value;
		RawImage ImageOk;
		RawImage ImageNg;
		foreach(GameObject checkListBox in checkListBoxes) {
			value = page * checkListBoxes.Length + count;
			ImageOk = checkListBox.transform.FindChild("Ok").gameObject.GetComponent<RawImage>();
			ImageNg = checkListBox.transform.FindChild("Ng").gameObject.GetComponent<RawImage>();
			if(value < checkTextList.Count) {
				if(japaneseMode) {
					checkListBox.GetComponent<Text>().text = checkTextJPWordList[ConvertWord(checkTextList[value])];
				}
				else {
					checkListBox.GetComponent<Text>().text = checkTextList[value];
				}

				if(playerStateList[GetPlayerName(player)].CheckList[checkTextList[value]]) {
					ViewerController.instance.ChangeRawImageState(ImageOk, true);
					ViewerController.instance.ChangeRawImageState(ImageNg, false);
				}
				else {
					ViewerController.instance.ChangeRawImageState(ImageOk, false);
					ViewerController.instance.ChangeRawImageState(ImageNg, true);
				}
			}
			else {
				checkListBox.GetComponent<Text>().text = "";
				ViewerController.instance.ChangeRawImageState(ImageOk, false);
				ViewerController.instance.ChangeRawImageState(ImageNg, false);
			}
			count++;
		}
	}

	/// <summary>
	/// Picks up check box.
	/// </summary>
	private void PickUpCheckBox() {
		string target = checkTextList[selectBox];
		if(japaneseMode) {
			pickUp.GetComponent<Text>().text = checkTextJPWordList[ConvertWord(target)];
			evaluation.GetComponent<Text>().text = checkTextJPEvaluationList[ConvertWord(target)];
		}
		else {
			pickUp.GetComponent<Text>().text = target;
			evaluation.GetComponent<Text>().text = checkTextEvaluationList[ConvertWord(target)];
		}
		ShowPreview(target);
	}

	/// <summary>
	/// Shows the preview.
	/// </summary>
	/// <param name="incidentName">Incident name.</param>
	private void ShowPreview(string incidentName) {
		string key = CameraController.instance.CreateKeyForScreenshot(GetPlayerName(player), incidentName);
		if(playerScreenshotList.ContainsKey(key)) {
			if(playerStateList[GetPlayerName(player)].CheckList[checkTextList[selectBox]]) {
				preview.GetComponent<RawImage>().texture = successTexture;
			}
			else {
				List<Texture2D> screenshotList = playerScreenshotList[key];
				StartCoroutine(LoopPreview(selectBox, key, screenshotList));
			}
		}
		else {
			preview.GetComponent<RawImage>().texture = noPreviewTexture;
		}
	}

	/// <summary>
	/// Loops the preview.
	/// </summary>
	/// <returns>The preview.</returns>
	/// <param name="nowSelectBox">Now select box.</param>
	/// <param name="key">Key.</param>
	/// <param name="screenshotList">Screenshot list.</param>
	private IEnumerator LoopPreview(int nowSelectBox, string key, List<Texture2D> screenshotList) {
		int count = 0;
		int length = screenshotList.Count;
		while(nowSelectBox == selectBox) {
			preview.GetComponent<RawImage>().texture = screenshotList[count];
			count++;
			if(count > length - 1) {
				count = 0;
			}
			yield return new WaitForSeconds(interval);
		}
	}

}
