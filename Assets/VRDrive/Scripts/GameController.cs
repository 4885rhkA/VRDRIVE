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

	[SerializeField] public bool oneKillMode = true;

	private Dictionary<string, UserSet> userSets = new Dictionary<string, UserSet>();

	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);
	private Color fontColor = new Color();
	private string colorReady = "#272629FF";
	private string colorGo = "#FFFFFFFF";
	private string colorMiss = "#FFFFFFFF";
	private string colorTimer = "#FFFFFFFF";
	private string colorSpeedMeter = "#FFFFFFFF";

	private bool startGameAtTheSameTimeFlag = false;
	private int remainingInGame = 0;

	void Awake() {
		instance = this;
		userSets.Clear ();
	}

	void Start() {
		GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car");
		GameObject checks = GameObject.Find ("Checks");
		UserSet userSet;
		UserObject userObject;
		UserState userState;

		StageController.instance.SetCondition ();

		if(carObjects != null) {
			// Insert data about User's operating Car
			foreach(GameObject carObject in carObjects) {
				if (carObject.name.Contains ("User")) {
					userSets.Add(carObject.name, new UserSet(new UserObject(carObject), new UserState()));
					remainingInGame++;
				}
			}

			foreach(KeyValuePair<string, UserSet> eachUserSet in userSets) {
				userSet = eachUserSet.Value;
				userObject = userSet.UserObject;
				userState = userSet.UserState;

				// Initialize CheckList
				if (checks != null) {
					foreach (Transform check in checks.transform) {
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
				ViewerController.instance.ChangeRawImageState(userObject.HowTo.GetComponent<RawImage>(), true);
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

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSets) {
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			// Update timer
			timerText = userObject.Timer.transform.FindChild("TimerText").gameObject.GetComponent<Text> ();
			if (ColorUtility.TryParseHtmlString (colorTimer, out fontColor)) {
				ViewerController.instance.ChangeTextContent(timerText, ViewerController.instance.GetTimerText (timeSpan), fontColor);
			}

			//Keep adding gravity
			UserController.instance.AddLocalGravity(userObject.Obj.GetComponent<Rigidbody>());
	
			// Update Speed Meter 
			if(userObject.SpeedMeter != null) {
				speed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();

				if (ColorUtility.TryParseHtmlString (colorSpeedMeter, out fontColor)) {
					ViewerController.instance.ChangeTextMeshContent(userObject.SpeedMeter.GetComponent<TextMesh>(), speed.ToString("f1"), fontColor);
				}
			}
		}
	}

	/// <summary>Start preparing playing game after press KeyCode E.</summary>
	private void ReleaseStartGame() {
		UserObject userObject;
		GameObject messageText;
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSets) {
			userObject = eachUserSet.Value.UserObject;

			// Standby for starting
			messageText = userObject.Message.transform.FindChild ("MessageText").gameObject;
			ViewerController.instance.ChangeRawImageState(userObject.HowTo.GetComponent<RawImage>(), false);
			ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), true);
			ViewerController.instance.ChangeTextState(0, messageText.GetComponent<Text>(), true);
			if(ColorUtility.TryParseHtmlString(colorReady, out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText.GetComponent<Text>(), "READY...", fontColor);
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
		foreach(KeyValuePair<string, UserSet> eachUserSet in userSets){
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;
			messageText = userObject.Message.transform.FindChild("MessageText").GetComponent<Text>();
			timerText = userObject.Timer.transform.FindChild ("TimerText").GetComponent<Text> ();

			UpdateUserStatus(userObject.Obj.name, 0);

			// Start Game
			ViewerController.instance.ChangeRawImageState(userObject.Timer.GetComponent<RawImage>(), true);
			ViewerController.instance.ChangeTextState(0, timerText, true);
			ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), false);
			if(ColorUtility.TryParseHtmlString(colorGo, out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText, "GO!", fontColor);
			}
			StartCoroutine(ViewerController.instance.ChangeTextState(SoundController.instance.GetClipLength("go"), messageText, false));
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
			UserSet userSet = userSets [userName];
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
	/// <param name="userName">The name for user</param>
	/// <param name="condition">The condition of each user</param>
	public void UpdateUserCondition(string userName, int condition) {
		if (HasUserSet (userName)) {
			UserSet userSet = userSets [userName];
			UserState userState = userSet.UserState;

			userState.Condition = condition;

			switch(condition) {
				case 1:
					StartCoroutine(ViewerController.instance.ChangeDamageView(userSets[userName].UserObject.MainCamera));
					break;
				default:
					break;
			}
		}
	}

	/// <summary>Update the finished time.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="timeSpan">PastTime from the starte</param>
	public void UpdateRecord(string userName, TimeSpan timeSpan) {
		if (HasUserSet (userName)) {
			userSets [userName].UserState.Record = timeSpan;
		}
	}

	public void UpdateCheckList(string userName, string checkName, bool value) {
		if (HasUserSet (userName)) {
			userSets [userName].UserState.CheckList [checkName] = value;
		}
	}

	/// <summary>Call the miss display.</summary>
	/// <param name="userName">The name for user</param>
	public void MissGame(string userName) {
		if (HasUserSet (userName)) {
			GameObject result = userSets[userName].UserObject.Result;
			Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();

			// Set View and Sound for Miss
			if(ColorUtility.TryParseHtmlString(colorMiss, out fontColor)) {
				ViewerController.instance.ChangeTextContent(resultText, "MISS......", fontColor);
			}
			ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
			ViewerController.instance.ChangeTextState(0, resultText, true);
			SoundController.instance.ShotClipSound("miss");
		}
	}

	/// <summary>Call the miss display quickly.</summary>
	/// <param name="userName">The name for user</param>
	private void MissGameQuickly(string userName) {
		if (HasUserSet (userName)) {
			UserSet userSet = userSets [userName];
			UserObject userObject = userSet.UserObject;
			UserState userState = userSet.UserState;

			userState.Record = TimerController.instance.PastTime;
			UpdateUserStatus(userObject.Obj.name, 2);
			MissGame(userObject.Obj.name);
		}
	}

	public void ChangeGameScene() {
		UserSet userSet;

		if(!startGameAtTheSameTimeFlag) {
			startGameAtTheSameTimeFlag = true;
			ReleaseStartGame();
		}

		if(remainingInGame == 0) {
			SceneManager.LoadScene("menu");
		}

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSets) {
			userSet = eachUserSet.Value;
			if(userSet.UserState.Status == 0) {
				MissGameQuickly(userSet.UserObject.Obj.name);
			}
		}
	}

	public bool HasUserSet(string name) {
		if (userSets.ContainsKey (name)) {
			return true;
		}
		return false;
	}

	public UserSet GetUserSet(string userName) {
		if (HasUserSet (userName)) {
			return userSets [userName];
		}
		return new UserSet(new UserObject(), new UserState());
	}

}
