using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>This script operate the game progress. </summary>
public class GameController : MonoBehaviour {
	
	/// <summary>This class contains the values for oprating parameters.</summary>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
	/// <param name="consition">The condition of each user(-1:courseout, 0:normal, 1:dash, 2:damage)</param>
	/// <param name="record">The time for goal</param>
	/// <param name="obj">Gameobject of the car</param>
	/// <param name="rigid">Rigidbody of the car</param>
	/// <param name="timerText">The text of the timer in upper left view</param>
	/// <param name="message">The Object in upper left view(Contains the Text in it)</param>
	public class UserState {
		public int status;
		public int condition;
		public TimeSpan record;
		public GameObject obj;
		public Rigidbody rigid;
		public Text timerText;
		public GameObject message;
		public UserState(GameObject carObject) {
			status = -1;
			condition = 0;
			record = new TimeSpan(0, 0, 0);
			obj = carObject;
			rigid = null;
			timerText = null;
			message = null;
		}
	}

	public static GameController instance;

	private GameObject[] carObjects;
	private Dictionary<string, UserState> cars = new Dictionary<string, UserState>();

	private TimerController timerController;
	private ViewerController viewerController;
	private UserController userController;
	private SoundController soundController;

	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);
	private Color fontColor = new Color();

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	void Start() {
		timerController = TimerController.instance;
		viewerController = ViewerController.instance;
		userController = UserController.instance;
		soundController = SoundController.instance;
		carObjects = GameObject.FindGameObjectsWithTag("Car");
		if(carObjects != null) {
			foreach(GameObject carObject in carObjects) {
				cars.Add(carObject.name, new UserState(carObject));
			}
			carObjects = null;

			UserState carValue;
			GameObject carValueObj;
			foreach(KeyValuePair<string, UserState> car in cars){
				carValue = car.Value;
				carValueObj = carValue.obj;
				carValue.rigid = carValueObj.GetComponent<Rigidbody>();
				carValue.timerText = carValueObj.FindDeep("TimerText").GetComponent<Text>();
				carValue.message = carValueObj.FindDeep("Message");
				viewerController.ChangeTextState(carValue.timerText, false);
				viewerController.ChangeRawImageState(carValue.message.GetComponent<RawImage>(), true);
				if(ColorUtility.TryParseHtmlString("#272629FF", out fontColor)) {
					viewerController.ChangeTextContent(carValue.message.FindDeep("MessageText").GetComponent<Text>(), "READY...", fontColor);
				}
				userController.RemoveDefaultGravity(carValue.rigid);
			}
			UpdateAllUserStatus(-1);
		}
		StartCoroutine("StartGame", soundController.GetClipLength("count"));
	}

	/// <summary>Start the game after finishing the count sound.</summary>
	/// <param name="countClipLength">The length of the count sound</param>
	private IEnumerator StartGame(float countClipLength) {  
		yield return new WaitForSeconds(countClipLength);
		UpdateAllUserStatus(0);
		timerController.ResetStartTime();
		UserState carValue;
		foreach(KeyValuePair<string, UserState> car in cars){
			carValue = car.Value;
			viewerController.ChangeTextState(carValue.timerText, true);
			viewerController.ChangeRawImageState(carValue.message.GetComponent<RawImage>(), false);
			if(ColorUtility.TryParseHtmlString("#FFFFFFFF", out fontColor)) {
				viewerController.ChangeTextContent(carValue.message.FindDeep("MessageText").GetComponent<Text>(), "GO!!", fontColor);
			}
			StartCoroutine(ChangeTextStateWithDelay(soundController.GetClipLength("go"), carValue.message.FindDeep("MessageText").GetComponent<Text>(), false));
		}
		soundController.StartStageSound();
	}

	/// <summary>Execute viewerController.ChangeTextState with delay</summary>
	/// <param name="delayLength">The length of the delay</param>
	/// <param name="text">The target Text Component</param>
	/// <param name="state">The trigger for showing text or not</param>
	private IEnumerator ChangeTextStateWithDelay(float delayLength, Text text, bool state) {  
		yield return new WaitForSeconds(delayLength);
		viewerController.ChangeTextState(text, state);
	}

	void Update() {
		timeSpan = timerController.pastTime;
		UserState carValue;
		foreach(KeyValuePair<string, UserState> car in cars){
			carValue = car.Value;
			viewerController.SetTimerTextToView(carValue.timerText, timeSpan);
			userController.AddLocalGravity(carValue.rigid);
		}
	}

	/// <summary>set the status for all users</summary>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
	public void UpdateAllUserStatus(int status) {
		foreach(KeyValuePair<string, UserState> car in cars){
			UpdateUserStatus(car.Value.obj.name, status);
		}
	}

	/// <summary>set the status</summary>
	/// <param name="carName">The name for User</param>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire, 3:incident)</param>
	public void UpdateUserStatus(string carName, int status) {
		if(cars.ContainsKey(carName)) {
			cars[carName].status = status;
			switch(status) {
				case -1:
					userController.SetFreezing(cars[carName].rigid);
					break;
				case 0:
					userController.ReleaseFreezing(cars[carName].rigid);
					break;
				case 1:
					GameObject message = cars[carName].message;
					cars[carName].record = gameObject.GetComponent<TimerController>().pastTime;
					viewerController.ChangeRawImageState(message.GetComponent<RawImage>(), true);
					if(ColorUtility.TryParseHtmlString("#EE4646FF", out fontColor)) {
						viewerController.ChangeTextContent(message.FindDeep("MessageText").GetComponent<Text>(), "GOAL!!", fontColor);
					}
					viewerController.ChangeTextState(message.FindDeep("MessageText").GetComponent<Text>(), true);
					soundController.GoalStageSound();
					break;
				case 3:
					break;
				default:
					break;
			}
		}
		else {
			Debug.LogWarning("The system cannot find the target:" + carName);
		}
	}

	/// <summary>
	/// update the status for User. 
	/// Moreover, the target of Tag for collision are Car/Gimmick.
	/// </summary>
	/// <param name="incidentObj">The Object occurs incident</param>
	/// <param name="targetObj">The Object suffered the incident</param>
	/// <returns>
	///  1:Change the status of user
	///  0:Collision both incident with the same tag or User is still in incident / Each incident must do only each defined action
	/// -1:Collision both incident with the different tag / No incident occurs
	/// </returns>
	public int UpdateGameState(GameObject incidentObj, GameObject targetObj) {
		if(targetObj.tag == "Car") {
			string targetObjName = targetObj.name;
			if(cars.ContainsKey(targetObjName)) {
				if(cars[targetObjName].status != 1 && incidentObj.name == "Goal") {
					UpdateUserStatus(targetObjName, 1);
					return 1;
				}
				else if(cars[targetObjName].status == 0) {
					UpdateUserStatus(targetObjName, 3);
					return 1;
				}
				else if(cars[targetObjName].status == 3) {
					return 0;
				}
			}
			else {
				Debug.LogWarning("The system cannot find the target:" + targetObjName);
			}
		}
		else if(incidentObj.tag == targetObj.tag) {
			return 0;
		}
		return -1;
	}

}
