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
	/// <param name="rank">The ranking for multiple battle(no implementation)</param>
	/// <param name="record">The time for goal</param>
	/// <param name="obj">Gameobject of the car</param>
	/// <param name="rigid">Rigidbody of the car</param>
	/// <param name="timerText">The text of the timer in upper left view</param>
	/// <param name="message">The Object in upper left view(Contains the Text in it)</param>
	public class UserState {
		public int status;
		public int rank;
		public int condition;
		public TimeSpan record;
		public GameObject obj;
		public Rigidbody rigid;
		public Text timerText;
		public GameObject message;
		public UserState(GameObject carObject) {
			status = -1;
			rank = 0;
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
			foreach(KeyValuePair<string, UserState> car in cars){
				car.Value.rigid = car.Value.obj.GetComponent<Rigidbody>();
				car.Value.timerText = car.Value.obj.FindDeep("TimerText").GetComponent<Text>();
				car.Value.message = car.Value.obj.FindDeep("Message");
				viewerController.ChangeTextState(car.Value.timerText, false);
				viewerController.ChangeRawImageState(car.Value.message.GetComponent<RawImage>(), true);
				if(ColorUtility.TryParseHtmlString("#272629FF", out fontColor)) {
					viewerController.ChangeTextContent(car.Value.message.FindDeep("MessageText").GetComponent<Text>(), "READY...", fontColor);
				}
				userController.RemoveDefaultGravity(car.Value.rigid);
			}
			ChangeAllStatus(-1);
		}
		StartCoroutine("StartGame", soundController.GetClipLength("count"));
	}

	/// <summary>Start the game after finishing the count sound.</summary>
	/// <param name="countClipLength">The length of the count sound</param>
	private IEnumerator StartGame(float countClipLength) {  
		yield return new WaitForSeconds(countClipLength);
		ChangeAllStatus(0);
		timerController.ResetStartTime();
		foreach(KeyValuePair<string, UserState> car in cars){
			viewerController.ChangeTextState(car.Value.timerText, true);
			viewerController.ChangeRawImageState(car.Value.message.GetComponent<RawImage>(), false);
			if(ColorUtility.TryParseHtmlString("#FFFFFFFF", out fontColor)) {
				viewerController.ChangeTextContent(car.Value.message.FindDeep("MessageText").GetComponent<Text>(), "GO!!", fontColor);
			}
			StartCoroutine(ChangeTextStateWithDelay(soundController.GetClipLength("go"), car.Value.message.FindDeep("MessageText").GetComponent<Text>(), false));
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
		timeSpan = timerController.getPastTime();
		foreach(KeyValuePair<string, UserState> car in cars){
			viewerController.SetTimerTextToView(car.Value.timerText, timeSpan);
			userController.AddLocalGravity(car.Value.rigid);
		}
	}

	/// <summary>set the status for all users</summary>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
	public void ChangeAllStatus(int status) {
		foreach(KeyValuePair<string, UserState> car in cars){
			ChangeStatus(car.Value.obj.name, status);
		}
	}

	/// <summary>set the status</summary>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire, 3:incident)</param>
	public void ChangeStatus(string carName, int status) {
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
					cars[carName].record = gameObject.GetComponent<TimerController>().getPastTime();
					viewerController.ChangeRawImageState(cars[carName].message.GetComponent<RawImage>(), true);
					if(ColorUtility.TryParseHtmlString("#EE4646FF", out fontColor)) {
						viewerController.ChangeTextContent(cars[carName].message.FindDeep("MessageText").GetComponent<Text>(), "GOAL!!", fontColor);
					}
					viewerController.ChangeTextState(cars[carName].message.FindDeep("MessageText").GetComponent<Text>(), true);
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

	/// <summary>Tag: Car/Gimmick/Item(Not implemented)</summary>
	public int UpdateGameState(GameObject incidentObj, GameObject targetObj) {
		if(targetObj.tag == "Car") {
			if(cars.ContainsKey(targetObj.name)) {
				if(cars[targetObj.name].status != 1 && incidentObj.name == "Goal") {
					ChangeStatus(targetObj.name, 1);
					return 1;
				}
				else if(cars[targetObj.name].status == 0) {
					ChangeStatus(targetObj.name, 3);
					return 1;
				}
			}
			else {
				Debug.LogWarning("The system cannot find the target:" + targetObj.name);
			}
		}
		else if(incidentObj.tag == targetObj.tag) {
			return 0;
		}
		return -1;
	}

}
