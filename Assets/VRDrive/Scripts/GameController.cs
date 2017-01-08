using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public struct UserSet{
	private UserObject userObject;
	private UserState userState;

	public UserSet(UserObject userObject, UserState userState) {
		this.userObject = userObject;
		this.userState = userState;
	}

	public UserObject UserObject {
		get {
			return userObject;
		}
	}

	public UserState UserState {
		get {
			return userState;
		}
	}

}

/// Control class for the each user's status
public class GameController : MonoBehaviour {

	public static GameController instance;

	[SerializeField] private bool oneKillMode = true;

	private Dictionary<string, UserSet> userSetList = new Dictionary<string, UserSet>();

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList = new Dictionary<string, string>() {
		{"ready", "#272629FF"},
		{"go", "#FFFFFFFF"},
		{"miss", "#FFFFFFFF"},
		{"timer", "#FFFFFFFF"},
		{"speedMeter", "#FFFFFFFF"},
		{"goal", "#FFFFFFFF"},
		{"record", "#FFFFFFFF"},
		{"result", "#FFFFFFFF"}
	};

	private Dictionary<string, string> messageList = new Dictionary<string, string>() {
		{"ready", "READY..."},
		{"go", "GO!"},
		{"miss", "MISS..."},
		{"goal", "GOAL!"},
		{"time", "TIME "},
	};

	private bool startGameAtTheSameTimeFlag = false;
	private int remainingInGame = 0;
	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);

	public bool OneKillMode {
		get {
			return oneKillMode;
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

	void Awake() {
		instance = this;
		userSetList.Clear ();
	}

	void Start() {
		GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
		GameObject checkList = GameObject.Find ("CheckList");
		UserSet userSet;
		UserObject userObject;
		UserState userState;

		StageController.instance.SetCondition ();

		if(carObjects != null) {
			// Insert data about User's operating Car
			foreach(GameObject carObject in carObjects) {
				userSetList.Add (carObject.name, new UserSet (new UserObject (carObject), new UserState ()));
				if (isPlayer(carObject.name)) {
					remainingInGame++;
				}
			}

			foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
				userSet = eachUserSet.Value;
				userObject = userSet.UserObject;
				userState = userSet.UserState;

				// Initialize CheckList
				if (checkList != null) {
					foreach (Transform check in checkList.transform) {
						// TODO find the way how to set
						if (check.name != "Stop") {
							userState.CheckList.Add (check.name, true);
						}
						else {
							userState.CheckList.Add (check.name, false);
						}
					}
				}

				// Set standby position
				if (isPlayer (eachUserSet.Key)) {
					ViewerController.instance.ChangeRawImageState(userObject.HowTo.GetComponent<RawImage>(), true);
				}
				UserController.instance.RemoveDefaultGravity(userObject.Obj.GetComponent<Rigidbody>());
				UpdateUserStatus(userObject.Obj.name, -1);
			}
		}
	}

	void Update() {
		UserSet userSet;
		UserObject userObject;
		Text timerText;
		float speed;
		timeSpan = TimerController.instance.PastTime;

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			if (isPlayer (eachUserSet.Key)) {
				// Update timer
				timerText = userObject.Timer.transform.FindChild("TimerText").gameObject.GetComponent<Text> ();
				if (ColorUtility.TryParseHtmlString (colorList["timer"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(timerText, ViewerController.instance.GetTimerText (timeSpan), fontColor);
				}

				// Update Speed Meter 
				if(userObject.SpeedMeter != null) {
					speed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();

					if (ColorUtility.TryParseHtmlString (colorList["speedMeter"], out fontColor)) {
						ViewerController.instance.ChangeTextMeshContent(userObject.SpeedMeter.GetComponent<TextMesh>(), speed.ToString("f1"), fontColor);
					}
				}
			}

			//Keep adding gravity
			UserController.instance.AddLocalGravity(userObject.Obj.GetComponent<Rigidbody>());

		}
	}

	/// <summary>Start preparing playing game after press KeyCode E.</summary>
	private void ReleaseStartGame() {
		UserObject userObject;
		GameObject messageText;
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userObject = eachUserSet.Value.UserObject;

			if (isPlayer (eachUserSet.Key)) {
				// Standby for starting
				messageText = userObject.Message.transform.FindChild ("MessageText").gameObject;
				ViewerController.instance.ChangeRawImageState(userObject.HowTo.GetComponent<RawImage>(), false);
				ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), true);
				StartCoroutine(ViewerController.instance.ChangeTextState(0, messageText.GetComponent<Text>(), true));
				if(ColorUtility.TryParseHtmlString(colorList["ready"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(messageText.GetComponent<Text>(), messageList["ready"], fontColor);
				}
			}
		}
		SoundController.instance.ShotClipSound("count");
		StartCoroutine(StartGame(SoundController.instance.GetClipLength("count")));
	}

	/// <summary>Start the game after finishing the count sound.</summary>
	/// <param name="clipLength">The length of the count <c>AudioClip</c></param>
	private IEnumerator StartGame(float clipLength) {  
		yield return new WaitForSeconds(clipLength);
		UserSet userSet;
		UserObject userObject;
		Text messageText;
		Text timerText;
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList){
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			UpdateUserStatus(userObject.Obj.name, 0);

			if (isPlayer (eachUserSet.Key)) {
				messageText = userObject.Message.transform.FindChild("MessageText").GetComponent<Text>();
				timerText = userObject.Timer.transform.FindChild ("TimerText").GetComponent<Text> ();

				// Start Game
				ViewerController.instance.ChangeRawImageState(userObject.Timer.GetComponent<RawImage>(), true);
				StartCoroutine(ViewerController.instance.ChangeTextState(0, timerText, true));
				ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), false);
				if(ColorUtility.TryParseHtmlString(colorList["go"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(messageText, messageList["go"], fontColor);
				}
				StartCoroutine(ViewerController.instance.ChangeTextState(SoundController.instance.GetClipLength("go"), messageText, false));
			}
		}
		TimerController.instance.ResetStartTime();
		SoundController.instance.StartStageSound();
		StageController.instance.StartGimmick ();
	}

	/// <summary>Update the status.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="status">The status of each user</param>
	public void UpdateUserStatus(string userName, int status) {
		if (HasUserSet (userName)) {
			UserSet userSet = userSetList [userName];
			UserObject userObject = userSet.UserObject;
			UserState userState = userSet.UserState;

			if (userState.Status < 1) {
				userState.Status = status;
				if(userState.Status > 0) {
					remainingInGame--;
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
	}

	/// <summary>Update the condition.</summary>
	/// <param name="userName">The <c>GameObject</c> name for user</param>
	/// <param name="condition">The condition of each user</param>
	public void UpdateUserCondition(string userName, int condition) {
		if (HasUserSet (userName)) {
			UserSet userSet = userSetList [userName];
			UserState userState = userSet.UserState;

			userState.Condition = condition;

			switch(condition) {
				case 1:
					StartCoroutine(ViewerController.instance.ChangeDamageView(userSetList[userName].UserObject.MainCamera));
					break;
				default:
					break;
			}
		}
	}

	/// <summary>Update the finished time.</summary>
	/// <param name="userName">The <c>GameObject</c> name for user</param>
	/// <param name="timeSpan">PastTime from the starte</param>
	public void UpdateRecord(string userName, TimeSpan timeSpan) {
		if (HasUserSet (userName)) {
			userSetList [userName].UserState.Record = timeSpan;
		}
	}

	/// <summary>Update the check list.</summary>
	/// <param name="userName">The <c>GameObject</c> name for user</param>
	/// <param name="checkName">The <c>GameObject</c> name for check</param>
	/// <param name="value">Keep the traffic rules or not</param>
	public void UpdateCheckList(string userName, string checkName, bool value) {
		if (HasUserSet (userName)) {
			userSetList [userName].UserState.CheckList [checkName] = value;
		}

		// Debug
		foreach(KeyValuePair<string, bool> check in userSetList [userName].UserState.CheckList) {
			Debug.Log (Time.time + " / " + check.Key + " : " + check.Value);
		}
	}

	/// <summary>Call the miss display.</summary>
	/// <param name="userName">The <c>GameObject</c> name for user</param>
	public void MissGame(string userName) {
		if (HasUserSet (userName)) {
			GameObject result = userSetList[userName].UserObject.Result;

			if (isPlayer (userName)) {
				Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();

				// Set View and Sound for Miss
				if(ColorUtility.TryParseHtmlString(colorList["miss"], out fontColor)) {
					ViewerController.instance.ChangeTextContent(resultText, messageList["miss"], fontColor);
				}
				ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
				StartCoroutine(ViewerController.instance.ChangeTextState(0, resultText, true));
				SoundController.instance.ShotClipSound("miss");
			}
		}
	}

	/// <summary>Call the miss display quickly.</summary>
	/// <param name="userName">The <c>GameObject</c> name for user</param>
	private void MissGameQuickly(string userName) {
		if (HasUserSet (userName)) {
			UserSet userSet = userSetList [userName];
			UserObject userObject = userSet.UserObject;
			UserState userState = userSet.UserState;

			userState.Record = TimerController.instance.PastTime;
			UpdateUserStatus(userObject.Obj.name, 2);
			MissGame(userObject.Obj.name);
		}
	}

	/// <summary>Call the scene.</summary>
	public void ChangeGameScene() {
		UserSet userSet;

		if(!startGameAtTheSameTimeFlag) {
			startGameAtTheSameTimeFlag = true;
			ReleaseStartGame();
		}

		if(remainingInGame == 0) {
			SceneManager.LoadScene("menu");
		}

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSetList) {
			userSet = eachUserSet.Value;
			if(userSet.UserState.Status == 0) {
				MissGameQuickly(userSet.UserObject.Obj.name);
			}
		}
	}

	/// <summary>Have UserSet or not.</summary>
	/// <param name="name">The <c>GameObject</c> name</param>
	/// <returns>Has UserSet or not</returns>
	public bool HasUserSet(string name) {
		if (userSetList.ContainsKey (name)) {
			return true;
		}
		return false;
	}

	/// <summary>Get UserSet.</summary>
	/// <param name="name">The <c>GameObject</c> name</param>
	/// <returns>UserSet</returns>
	public UserSet GetUserSet(string name) {
		if (HasUserSet (name)) {
			return userSetList [name];
		}
		return new UserSet(new UserObject(), new UserState());
	}

	/// <summary>Have UserSet or not.</summary>
	/// <param name="name">The <c>GameObject</c> name</param>
	/// <returns>Name is play or not</returns>
	public bool isPlayer(string name) {
		if (HasUserSet (name) && name.Contains("Player")) {
			return true;
		}
		return false;
	}

}
