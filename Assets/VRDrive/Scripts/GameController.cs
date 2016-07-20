using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>This script operate the game progress. </summary>
public class GameController : MonoBehaviour {
	
	/// <summary>This class contains the values for oprating parameters.</summary>
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
	/// <param name="rank">The ranking for multiple battle(no implementation)</param>
	/// <param name="record">The time for goal</param>
	/// <param name="obj">Gameobject of the car</param>
	/// <param name="rigid">Rigidbody of the car</param>
	/// <param name="timerText">The text of the timer in upper left view</param>
	/// <param name="message">The Object in upper left view(Contains the Text in it)</param>
	public class UserState {
		public int status;
		public int rank;
		public TimeSpan record;
		public GameObject obj;
		public Rigidbody rigid;
		public Text timerText;
		public GameObject message;
		public UserState(GameObject carObject) {
			status = -1;
			rank = 0;
			record = new TimeSpan(0, 0, 0);
			obj = carObject;
			rigid = null;
			timerText = null;
			message = null;
		}
	}

	private GameObject[] carObjects;
	private Dictionary<string, UserState> cars = new Dictionary<string, UserState>();

	private TimerController timerController;
	private ViewerController viewerController;
	private UserController userController;
	private SoundController soundController;

	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);
	private Color fontColor = new Color();

	void Start() {
		timerController = gameObject.GetComponent<TimerController>();
		viewerController = gameObject.GetComponent<ViewerController>();
		userController = gameObject.GetComponent<UserController>();
		soundController = gameObject.GetComponent<SoundController>();
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
				if(ColorUtility.TryParseHtmlString("#272629FF", out fontColor)) {
					viewerController.ChangeTextContent(car.Value.message.FindDeep("MessageText").GetComponent<Text>(), "READY...", fontColor);
				}
				userController.RemoveDefaultGravity(car.Value.rigid);
			}
			ChangeAllStatus(-1);
		}
		StartCoroutine("StartGame", soundController.getCountClipLength());
	}

	/// <summary>Start the game after finishing the count sound.</summary>
	/// <param name="countClipLength">The length of the count sound</param>
	private IEnumerator StartGame(float countClipLength) {  
		yield return new WaitForSeconds(countClipLength);
		ChangeAllStatus(0);
		timerController.ResetStartTime();
		foreach(KeyValuePair<string, UserState> car in cars){
			viewerController.ChangeTextState(car.Value.timerText, true);
			if(ColorUtility.TryParseHtmlString("#EE4646FF", out fontColor)) {
				viewerController.ChangeTextContent(car.Value.message.FindDeep("MessageText").GetComponent<Text>(), "GO!!", fontColor);
			}
		}
		soundController.StartStageSound();
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
	/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
	public void ChangeStatus(String carName, int status) {
		switch(status) {
			case -1:
				if(cars.ContainsKey(carName)) {
					cars[carName].status = -1;
					userController.SetFreezing(cars[carName].rigid);
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
			case 0:
				if(cars.ContainsKey(carName)) {
					cars[carName].status = 0;
					userController.ReleaseFreezing(cars[carName].rigid);
					viewerController.ChangeTextState(cars[carName].timerText, true);
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
			case 1:
				if(cars.ContainsKey(carName)) {
					cars[carName].status = 1;
					cars[carName].record = gameObject.GetComponent<TimerController>().getPastTime();
					if(ColorUtility.TryParseHtmlString("#EE4646FF", out fontColor)) {
						viewerController.ChangeTextContent(cars[carName].message.FindDeep("MessageText").GetComponent<Text>(), "GOAL!!", fontColor);
					}
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
		}
	}
}
