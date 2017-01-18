using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Game controller.
/// </summary>
public class GameController : MonoBehaviour {

	public static GameController instance;

	[SerializeField] private bool oneKillMode = true;
	[SerializeField] private bool keyboardMode = false;
	[SerializeField] private bool handleMode = true;
	[SerializeField] private bool pedalMode = true;
	[SerializeField] private bool pedalTwoMode = true;
	[SerializeField] private bool warningMode = false;
	[SerializeField] private bool warningWithPreviewMode = false;
	[SerializeField] private bool evaluationMode = false;
	[SerializeField] private bool timeAttackMode = true;
	[SerializeField] private string afterScene = "menu";
	[SerializeField] private GameObject valueKeeper = null;
	private Dictionary<string, UserSet> userSetList = new Dictionary<string, UserSet>();

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{ "ready", "#272629FF" },
		{ "go", "#FFFFFFFF" },
		{ "miss", "#FFFFFFFF" },
		{ "timer", "#FFFFFFFF" },
		{ "speedMeter", "#272629FF" },
		{ "goal", "#272629FF" },
		{ "record", "#FFFFFFFF" },
		{ "result", "#FFFFFFFF" }
	};

	public bool PedalTwoMode {
		get {
			return pedalTwoMode;
		}
	}

	public bool WarningMode {
		get {
			return warningMode;
		}
	}

	private Dictionary<string, string> messageList = new Dictionary<string, string>() {
		{ "ready", "READY..." },
		{ "go", "GO!" },
		{ "miss", "MISS..." },
		{ "goal", "GOAL!" },
		{ "time", "TIME " },
	};

	private bool startGameFlag = false;
	private bool finishGameFlag = false;
	private bool exitGameFlag = false;
	private bool replayFlag = false;

	private int remainingInGame = 0;

	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);

	public bool OneKillMode {
		get {
			return oneKillMode;
		}
	}

	public bool HandleMode {
		get {
			return handleMode;
		}
	}

	public bool KeyboardMode {
		get {
			return keyboardMode;
		}
	}

	public bool PedalMode {
		get {
			return pedalMode;
		}
	}

	public Dictionary<string, string> ColorList {
		get {
			return colorList;
		}
	}

	public Dictionary<string, string> MessageList {
		get {
			return messageList;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
		userSetList.Clear();
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
		GameObject checkList = GameObject.Find("CheckList");
		UserSet userSet;
		UserObject userObject;
		UserState userState;

		StageController.instance.SetCondition();

		if(carObjects != null) {
			// Insert data about User's operating Car
			foreach(GameObject carObject in carObjects) {
				userSetList.Add(carObject.name, new UserSet(new UserObject(carObject), new UserState()));
				if(IsPlayer(carObject.name)) {
					remainingInGame++;
				}
			}

			foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
				userSet = eachUserSet.Value;
				userObject = userSet.UserObject;
				userState = userSet.UserState;

				// Initialize CheckList
				if(checkList != null) {
					foreach(Transform check in checkList.transform) {
						if(check.name != "Stop") {
							userState.CheckList.Add(check.name, true);
						}
						else {
							userState.CheckList.Add(check.name, false);
						}
					}
				}

				// Set standby position
				if(IsPlayer(eachUserSet.Key)) {
					RawImage image = userObject.Image.GetComponent<RawImage>();
					ViewerController.instance.ChangeImageContent(image, "Images/Game/Attentions/press");
					ViewerController.instance.ChangeRawImageState(image, true);
				}
				UserController.instance.RemoveDefaultGravity(userObject.Obj.GetComponent<Rigidbody>());
				UpdateUserStatus(userObject.Obj.name, -1);
			}
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		UserSet userSet;
		UserObject userObject;
		Text timerText;
		float speed;
		timeSpan = TimerController.instance.PastTime;

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			if(IsPlayer(eachUserSet.Key)) {
				// Update timer
				if(timeAttackMode) {
					timerText = userObject.Timer.transform.FindChild("TimerText").gameObject.GetComponent<Text>();
					if(ColorUtility.TryParseHtmlString(colorList["timer"], out fontColor)) {
						ViewerController.instance.ChangeTextContent(timerText, ViewerController.instance.GetTimerText(timeSpan), fontColor);
					}
				}

				// Update Speed Meter 
				if(userObject.SpeedMeter != null) {
					speed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();

					if(ColorUtility.TryParseHtmlString(colorList["speedMeter"], out fontColor)) {
						ViewerController.instance.ChangeTextMeshContent(userObject.SpeedMeter.GetComponent<TextMesh>(), speed.ToString("f0"), fontColor);
					}
				}
			}

			// Keep adding gravity
			UserController.instance.AddLocalGravity(userObject.Obj.GetComponent<Rigidbody>());

		}
	}

	/// <summary>
	/// Releases the start game.
	/// </summary>
	private void ReleaseStartGame() {
		UserObject userObject;
		GameObject messageText;
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userObject = eachUserSet.Value.UserObject;

			if(IsPlayer(eachUserSet.Key)) {
				// Standby for starting
				messageText = userObject.Message.transform.FindChild("MessageText").gameObject;
				ViewerController.instance.ChangeRawImageState(userObject.Image.GetComponent<RawImage>(), false);
				ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), true);
				StartCoroutine(ViewerController.instance.ChangeTextState(messageText.GetComponent<Text>(), true));
				if(ColorUtility.TryParseHtmlString(colorList["ready"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(messageText.GetComponent<Text>(), messageList["ready"], fontColor);
				}
			}
		}
		SoundController.instance.ShotClipSound("count");
		StartCoroutine(StartGame(SoundController.instance.GetClipLength("count")));
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	/// <returns>The game.</returns>
	/// <param name="clipLength">Clip length.</param>
	private IEnumerator StartGame(float clipLength) {  
		yield return new WaitForSeconds(clipLength);
		UserSet userSet;
		UserObject userObject;
		Text messageText;
		Text timerText;
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			UpdateUserStatus(userObject.Obj.name, 0);

			if(IsPlayer(eachUserSet.Key)) {
				// Start Game
				if(timeAttackMode) {
					timerText = userObject.Timer.transform.FindChild("TimerText").GetComponent<Text>();
					ViewerController.instance.ChangeRawImageState(userObject.Timer.GetComponent<RawImage>(), true);
					StartCoroutine(ViewerController.instance.ChangeTextState(timerText, true));
				}
				messageText = userObject.Message.transform.FindChild("MessageText").GetComponent<Text>();
				ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), false);
				if(ColorUtility.TryParseHtmlString(colorList["go"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(messageText, messageList["go"], fontColor);
				}
				StartCoroutine(ViewerController.instance.ChangeTextState(messageText, false, SoundController.instance.GetClipLength("go")));
			}
		}
		TimerController.instance.ResetStartTime();
		SoundController.instance.StartGameSound();
		StageController.instance.StartGimmick();
	}

	/// <summary>
	/// Clears the game.
	/// </summary>
	/// <param name="userName">User name.</param>
	public void ClearGame(string userName) {
		UserSet userSet = userSetList[userName];
		UserObject userObject = userSet.UserObject;

		UpdateUserStatus(userObject.Obj.name, 1);
		UpdateRecord(userObject.Obj.name, TimerController.instance.PastTime);

		if(IsPlayer(userObject.Obj.name)) {
			Text messageText = userObject.Message.transform.FindChild("MessageText").GetComponent<Text>();
			if(ColorUtility.TryParseHtmlString(colorList["goal"], out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText, messageList["goal"], fontColor);
			}
			ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), true);
			StartCoroutine(ViewerController.instance.ChangeTextState(messageText, true));
			SoundController.instance.ShotClipSound("goal");
		}
	}

	/// <summary>
	/// Misses the game.
	/// </summary>
	/// <param name="userName">User name.</param>
	public void MissGame(string userName) {
		UserSet userSet = userSetList[userName];
		UserObject userObject = userSet.UserObject;

		UpdateUserStatus(userObject.Obj.name, 2);
		UpdateRecord(userObject.Obj.name, TimerController.instance.PastTime);

		if(IsPlayer(userObject.Obj.name)) {
			Text resultText = userObject.Result.transform.FindChild("ResultText").GetComponent<Text>();

			// Set View and Sound for Miss
			if(ColorUtility.TryParseHtmlString(colorList["miss"], out fontColor)) {
				ViewerController.instance.ChangeTextContent(resultText, messageList["miss"], fontColor);
			}
			ViewerController.instance.ChangeRawImageState(userObject.Result.GetComponent<RawImage>(), true);
			StartCoroutine(ViewerController.instance.ChangeTextState(resultText, true));
			SoundController.instance.ShotClipSound("miss");
		}
	}

	/// <summary>
	/// Changes the game scene. Moreover only player0 can use this function.
	/// </summary>
	/// <param name="playerName">Player name.</param>
	public void ChangeGameScene(string playerName) {
		UserSet userSet;

		if(!startGameFlag) {
			startGameFlag = true;
			ReleaseStartGame();
		}

		if(!exitGameFlag) {
			if(finishGameFlag) {
				exitGameFlag = true;
				if(evaluationMode) {
					KeepValuesToNextScene();
					SceneManager.LoadScene("evaluation");
				}
				else {
					SceneManager.LoadScene(afterScene);
				}
			}
			else if(playerName.Contains("0")) {
				foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
					userSet = eachUserSet.Value;
					if(userSet.UserState.Status == 0) {
						MissGame(userSet.UserObject.Obj.name);
					}
				}
			}
		}
	}

	/// <summary>
	/// Keeps the values to next scene.
	/// </summary>
	private void KeepValuesToNextScene() {
		GameObject newValueKeeper = Instantiate(valueKeeper, transform.position, transform.rotation) as GameObject;
		newValueKeeper.name = valueKeeper.name;

		Dictionary<string, UserState> userStateList = new Dictionary<string, UserState>();
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userStateList.Add(eachUserSet.Key, eachUserSet.Value.UserState);
		}

		newValueKeeper.GetComponent<ValueKeeper>().UserStateList = userStateList;
		newValueKeeper.GetComponent<ValueKeeper>().PlayerScreenshotList = CameraController.instance.PlayerScreenshotList;
		newValueKeeper.GetComponent<ValueKeeper>().SceneAfterEvaluation = afterScene;
		DontDestroyOnLoad(newValueKeeper);
	}

	/// <summary>
	/// Updates the user status.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="status">Status.</param>
	public void UpdateUserStatus(string userName, int status) {
		UserSet userSet = userSetList[userName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		userState.Status = status;

		if(IsPlayer(userObject.Obj.name) && userState.Status > 0) {
			remainingInGame--;
			if(remainingInGame == 0) {
				finishGameFlag = true;
			}
		}
		switch(userState.Status) {
			case -1:
				UserController.instance.SetFreezingPosition(userObject.Obj.GetComponent<Rigidbody>());
				break;
			case 0:
				UserController.instance.ReleaseFreezingPosition(userObject.Obj.GetComponent<Rigidbody>());
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Updates the user condition.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="condition">Condition.</param>
	public void UpdateUserCondition(string userName, int condition) {
		UserSet userSet = userSetList[userName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		userState.Condition = condition;

		switch(condition) {
			case 1:
				StartCoroutine(ViewerController.instance.ChangeDamageView(userObject.MainCamera));
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Updates the record.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="timeSpan">Time span.</param>
	public void UpdateRecord(string userName, TimeSpan timeSpan) {
		userSetList[userName].UserState.Record = timeSpan;
	}

	/// <summary>
	/// Updates the check list.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="checkName">Check name.</param>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void UpdateCheckList(string userName, string checkName, bool value) {
		UserState userState = userSetList[userName].UserState;
		if(userState.CheckList.ContainsKey(checkName)) {
			userState.CheckList[checkName] = value;
		}
	}

	/// <summary>
	/// Gets the check.
	/// </summary>
	/// <returns><c>true</c>, if check was gotten, <c>false</c> otherwise.</returns>
	/// <param name="userName">User name.</param>
	/// <param name="checkName">Check name.</param>
	public bool GetCheck(string userName, string checkName) {
		UserState userState = userSetList[userName].UserState;
		if(userState.CheckList.ContainsKey(checkName)) {
			return userState.CheckList[checkName];
		}
		else {
			Debug.LogWarning(checkName + " is not contained in " + userName);
			return false;
		}
	}

	/// <summary>
	/// Determines whether this instance has user set the specified name.
	/// </summary>
	/// <returns><c>true</c> if this instance has user set the specified name; otherwise, <c>false</c>.</returns>
	/// <param name="name">Name.</param>
	public bool HasUserSet(string name) {
		if(userSetList.ContainsKey(name)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Gets the user set.
	/// </summary>
	/// <returns>The user set.</returns>
	/// <param name="name">Name.</param>
	public UserSet GetUserSet(string name) {
		if(HasUserSet(name)) {
			return userSetList[name];
		}
		return new UserSet(new UserObject(), new UserState());
	}

	/// <summary>
	/// Determines whether this instance is player the specified name.
	/// </summary>
	/// <returns><c>true</c> if this instance is player the specified name; otherwise, <c>false</c>.</returns>
	/// <param name="name">Name.</param>
	public bool IsPlayer(string name) {
		if(HasUserSet(name) && name.Contains("Player")) {
			return true;
		}
		return false;
	}

	public void ShowWarning(string playerName, string checkName) {
		UserSet userSet = userSetList[playerName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		ViewerController.instance.ChangeImageContent(userObject.Image.GetComponent<RawImage>(), "Images/Game/Warnings/" + checkName);
		ViewerController.instance.ChangeRawImageState(userObject.Image.GetComponent<RawImage>(), true);

		RawImage replayImage = userObject.Obj.transform.FindChild("Canvas/ReplayImage").gameObject.GetComponent<RawImage>();
		ShowPreview(playerName, checkName, replayImage);
		StartCoroutine(CloseWarning(playerName, replayImage, 5f));
	}

	private void ShowPreview(string playerName, string checkName, RawImage replayImage) {
		UserSet userSet = userSetList[playerName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		string key = CameraController.instance.CreateKeyForScreenshot(playerName, checkName);
		List<Texture2D> screenshotList = CameraController.instance.PlayerScreenshotList[key];
		replayFlag = true;

		StartCoroutine(LoopAction(replayImage, screenshotList, userObject.Obj.GetComponent<Rigidbody>()));

	}

	private IEnumerator LoopAction(RawImage replayImage, List<Texture2D> screenshotList, Rigidbody rigid) {
		int count = 0;
		int length = screenshotList.Count;
		ViewerController.instance.ChangeRawImageState(replayImage, true);
		while(replayFlag) {
			// show preview
			if(warningWithPreviewMode) {
				replayImage.texture = screenshotList[count];
				count++;
				if(count > length - 1) {
					count = 0;
				}
			}
			rigid.velocity = Vector3.zero;
			rigid.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator CloseWarning(string playerName, RawImage replayImage, float delay) {  
		yield return new WaitForSeconds(delay);
		UserSet userSet = userSetList[playerName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		ViewerController.instance.ChangeRawImageState(replayImage, false);
		ViewerController.instance.ChangeRawImageState(userObject.Image.GetComponent<RawImage>(), false);

		replayFlag = false;
	}


}
