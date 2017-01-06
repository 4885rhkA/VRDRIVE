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

	private bool startGameAtTheSameTimeFlag = false;
	private int remainingInGame = 0;

	public UserSet GetUserSet(string userName) {
		if (userSets.ContainsKey (userName)) {
			return userSets [userName];
		}
		return new UserSet(new UserObject(), new UserState());
	}

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

		// TODO move this program or create other function
		if(CrossPlatformInputManager.GetButtonUp("Decide")) {
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

		foreach(KeyValuePair<string, UserSet> eachUserSet in userSets) {
			userSet = eachUserSet.Value;
			userObject = userSet.UserObject;

			// Update timer
			timerText = userObject.Timer.transform.FindChild("TimerText").gameObject.GetComponent<Text> ();
			ViewerController.instance.ChangeTextContent(timerText, ViewerController.instance.GetTimerText (timeSpan), fontColor);

			//Keep adding gravity
			UserController.instance.AddLocalGravity(userObject.Obj.GetComponent<Rigidbody>());
	
			// Update Speed Meter 
			if(userObject.SpeedMeter != null) {
				speed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();

				// TODO using viewcontroller
				userObject.SpeedMeter.GetComponent<TextMesh>().text = speed.ToString("f1");
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
			ViewerController.instance.ChangeTextState(messageText.GetComponent<Text>(), true);
			if(ColorUtility.TryParseHtmlString(colorReady, out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText.GetComponent<Text>(), "READY...", fontColor);
			}
		}
        SoundController.instance.ShotClipSound("count");
        StartCoroutine(StartGame(SoundController.instance.GetClipLength("count")));
	}

	public void UpdateCheckList(string userName, string checkName, bool value) {
		// TODO change the how to solve
		userSets [userName].UserState.CheckList [checkName] = value;
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
			ViewerController.instance.ChangeTextState(timerText, true);
			ViewerController.instance.ChangeRawImageState(userObject.Message.GetComponent<RawImage>(), false);
			if(ColorUtility.TryParseHtmlString(colorGo, out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText, "GO!", fontColor);
			}
			StartCoroutine(ChangeTextStateWithDelay(SoundController.instance.GetClipLength("go"), messageText, false));
		}
		TimerController.instance.ResetStartTime();
		SoundController.instance.StartStageSound();
		StageController.instance.StartGimmick ();
	}

	/// <summary>Execute viewerController.ChangeTextState with delay.</summary>
	/// <param name="delayLength">The length of the delay</param>
	/// <param name="messsageText">The target <c>Text</c> component</param>
	/// <param name="state">The trigger for showing text or not</param>
	private IEnumerator ChangeTextStateWithDelay(float delayLength, Text messageText, bool state) {  
		yield return new WaitForSeconds(delayLength);
		ViewerController.instance.ChangeTextState(messageText, state);
	}

	/// <summary>Update the status.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="userStatus">The status of each user</param>
	/// TODO Fix this algorithm
	public void UpdateUserStatus(string userName, int userStatus) {
		UserSet userSet = userSets [userName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;
		if(userState.Status < 1) {
			userState.Status = userStatus;
			if(userState.Status > 0) {
				remainingInGame--;
			}
		}
		switch(userStatus) {
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
	/// Get the value for deciding each object should do function or not when touching with them. 
	/// </summary>
	/// <param name="incidentObject">The <c>GameObject</c> occurs incident</param>
	/// <param name="targetObject">The <c>GameObject</c> suffered the incident</param>
	/// <returns>
	///  	1:IncidentObject and targetObject should do each functions
	///  	0:TargetObject should do function
	/// 	-1:No Objects should do
	/// </returns>
	/// TODO Fix this algorithm / It's bad to get value with updating user's condition
	public int GetOrderCodeForObjectsByTouch(GameObject incidentObject, GameObject targetObject) {
		if(targetObject.tag == "Car") {
			string targetObjectName = targetObject.name;
			if(userSets.ContainsKey(targetObjectName)) {
				if(userSets[targetObjectName].UserState.Status == 0) {
					if (incidentObject.tag == "Base") {
						if(incidentObject.name == "Goal" || incidentObject.name == "UnderGround") {
							return 1;
						}
						else if(incidentObject.name == "Start") {
							return -1;
						}
					}
					else if(incidentObject.tag == "Gimmick") {
						int userCondition = 1;
						if(userSets[targetObjectName].UserState.Condition == 0) {
							if(incidentObject.name == "SpeedUpBoards") {
								userCondition = 2;
							}
							UpdateUserCondition(targetObjectName, userCondition);
							return 1;
						}
						else if(userSets[targetObjectName].UserState.Condition == 2) {
							if(incidentObject.name == "SpeedUpBoards") {
								return -1;
							}
							UpdateUserCondition(targetObjectName, userCondition);
							return 1;
						}
					}
					else if(incidentObject.tag == "Check"){
						return -1;
					}
				}
				return 0;
			}
		}
		else if(incidentObject.tag == targetObject.tag) {
			return 0;
		}
		return -1;
	}

	/// <summary>Update the condition.</summary>
	/// <param name="carName">The name for user</param>
	/// <param name="carCondition">The condition of each user</param>
	/// TODO Fix this algorithm
	public void UpdateUserCondition(string carName, int carCondition) {
		if(userSets.ContainsKey(carName)) {
			userSets[carName].UserState.Condition = carCondition;
			switch(carCondition) {
				case 1:
					StartCoroutine(ViewerController.instance.ChangeDamageView(userSets[carName].UserObject.MainCamera));
					break;
				default:
					break;
			}
		}
		else {
			Debug.LogWarning("The system cannot find the target:" + carName);
		}
	}

	/// <summary>Call the miss display.</summary>
	/// <param name="userName">The name for user</param>
	public void MissGame(string userName) {
		GameObject result = userSets[userName].UserObject.Result;
		Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();

		// Set View and Sound for Miss
		if(ColorUtility.TryParseHtmlString(colorMiss, out fontColor)) {
			ViewerController.instance.ChangeTextContent(resultText, "MISS......", fontColor);
		}
		ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(resultText, true);
		SoundController.instance.ShotClipSound("miss");
	}

	/// <summary>Call the miss display quickly.</summary>
	/// <param name="userName">The name for user</param>
	private void MissGameQuickly(string userName) {
		UserSet userSet = userSets [userName];
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		userState.Record = TimerController.instance.PastTime;
		UpdateUserStatus(userObject.Obj.name, 2);
		MissGame(userObject.Obj.name);
	}

	/// <summary>Change the vignette in view.</summary>
	/// <param name="resultText">User's showing <c>Text</c> </param>
	/// <param name="resultTimeTextArray">Array of char for showing number one by one</param>
	public IEnumerator AddCharacterContinuouslyForResult(Text resultText, char[] resultTimeTextArray) {
		float clipLength = SoundController.instance.GetClipLength("record");

		foreach(char resultTimeText in resultTimeTextArray) {
			yield return new WaitForSeconds(clipLength);
			string newResultTimeText = resultText.text + resultTimeText;
			ViewerController.instance.ChangeTextContent(resultText, newResultTimeText, fontColor);
			SoundController.instance.ShotClipSound("record");
		}
	}

	/// <summary>Update the finished time.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="timeSpan">PastTime from the starte</param>
	public void UpdateRecord(string userName, TimeSpan timeSpan) {
		userSets [userName].UserState.Record = timeSpan;
	}

}
