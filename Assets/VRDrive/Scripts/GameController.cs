using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// Control class for the each user's status
public class GameController : MonoBehaviour {

	public static GameController instance;

	[SerializeField] public bool oneKillMode = true;

	private GameObject[] carObjects;
	public static Dictionary<string, UserState> cars = new Dictionary<string, UserState>();

	private TimeSpan timeSpan = new TimeSpan(0, 0, 0);
	private Color fontColor = new Color();
	private string colorReady = "#272629FF";
	private string colorGo = "#FFFFFFFF";

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
		carObjects = GameObject.FindGameObjectsWithTag("Car");
		if(carObjects != null) {
			foreach(GameObject carObject in carObjects) {
				cars.Add(carObject.name, new UserState(carObject));
			}
			carObjects = null;

			UserState carValue;
			foreach(KeyValuePair<string, UserState> car in cars){
				carValue = car.Value;
				ViewerController.instance.ChangeTextState(carValue.timer.transform.FindChild("TimerText").GetComponent<Text>(), false);
				ViewerController.instance.ChangeRawImageState(carValue.message.GetComponent<RawImage>(), true);
				if(ColorUtility.TryParseHtmlString(colorReady, out fontColor)) {
					ViewerController.instance.ChangeTextContent(carValue.message.transform.FindChild("MessageText").GetComponent<Text>(), "READY...", fontColor);
				}
				else {
					Debug.LogWarning("The color" + colorReady + "cannnot convert into Color class.");
				}
				UserController.instance.RemoveDefaultGravity(carValue.rigid);
				CameraController.instance.ChaseCar(carValue.camera.transform, carValue.obj.transform);
			}
			UpdateAllUserStatus(-1);
		}
		StartCoroutine(StartGame(SoundController.instance.GetClipLength("count")));
	}

	void Update() {
		timeSpan = TimerController.instance.pastTime;
		UserState carValue;
		foreach(KeyValuePair<string, UserState> car in cars) {
			carValue = car.Value;
			carValue.timer.transform.FindChild("TimerText").GetComponent<Text>().text = ViewerController.instance.GetTimerText(timeSpan);
			UserController.instance.AddLocalGravity(carValue.rigid);
			CameraController.instance.ChaseCar(carValue.camera.transform, carValue.obj.transform);
		}
	}

	/// <summary>Start the game after finishing the count sound.</summary>
	/// <param name="clipLength">The length of the count sound</param>
	private IEnumerator StartGame(float clipLength) {  
		yield return new WaitForSeconds(clipLength);
		UpdateAllUserStatus(0);
		TimerController.instance.ResetStartTime();
		UserState carValue;
		GameObject carMessage;
		Text carMessageText;
		foreach(KeyValuePair<string, UserState> car in cars){
			carValue = car.Value;
			carMessage = carValue.message;
			carMessageText = carMessage.transform.FindChild("MessageText").GetComponent<Text>();
			ViewerController.instance.ChangeTextState(carValue.timer.transform.FindChild("TimerText").GetComponent<Text>(), true);
			ViewerController.instance.ChangeRawImageState(carMessage.GetComponent<RawImage>(), false);
			if(ColorUtility.TryParseHtmlString(colorGo, out fontColor)) {
				ViewerController.instance.ChangeTextContent(carMessageText, "GO!!", fontColor);
			}
			else {
				Debug.LogWarning("The color" + colorGo + "cannnot convert into Color class.");
			}
			StartCoroutine(ChangeTextStateWithDelay(SoundController.instance.GetClipLength("go"), carMessageText, false));
		}
		SoundController.instance.StartStageSound();
	}

	/// <summary>Execute viewerController.ChangeTextState with delay.</summary>
	/// <param name="delayLength">The length of the delay</param>
	/// <param name="carMessageText">The target Text Component</param>
	/// <param name="carState">The trigger for showing text or not</param>
	private IEnumerator ChangeTextStateWithDelay(float delayLength, Text carMessageText, bool carState) {  
		yield return new WaitForSeconds(delayLength);
		ViewerController.instance.ChangeTextState(carMessageText, carState);
	}

	/// <summary>Update the status for all users.</summary>
	/// <param name="carStatus">The status of each user</param>
	public void UpdateAllUserStatus(int carStatus) {
		foreach(KeyValuePair<string, UserState> car in cars){
			UpdateUserStatus(car.Value.obj.name, carStatus);
		}
	}

	/// <summary>Update the status.</summary>
	/// <param name="carName">The name for User</param>
	/// <param name="carStatus">The status of each user</param>
	public void UpdateUserStatus(string carName, int carStatus) {
		if(cars.ContainsKey(carName)) {
			if(cars[carName].status!= 1) {
				cars[carName].status = carStatus;
			}
			switch(carStatus) {
				case -1:
					UserController.instance.SetFreezingPosition(cars[carName].rigid);
					break;
				case 0:
					UserController.instance.ReleaseFreezingPosition(cars[carName].rigid);
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
	/// Update the status for User. 
	/// Moreover, the target of Tag for collision are Car/Gimmick.
	/// </summary>
	/// <param name="incidentObject">The Object occurs incident</param>
	/// <param name="targetObject">The Object suffered the incident</param>
	/// <returns>
	///  	1:Change the status of user /
	///  	0:Collision both incident with the same tag or User is still in incident, Each incident must do only each defined action /
	/// 	-1:Collision both incident with the different tag, No incident occurs
	/// </returns>
	public int UpdateGameState(GameObject incidentObject, GameObject targetObject) {
		if(targetObject.tag == "Car") {
			string targetObjectName = targetObject.name;
			if(cars.ContainsKey(targetObjectName)) {
				if(cars[targetObjectName].status == 0) {
					if(incidentObject.name == "Goal" || incidentObject.name == "UnderGround") {
						return 1;
					}
					else if(incidentObject.name == "Start") {
						return -1;
					}
					else {
						int userCondition = 1;
						if(cars[targetObjectName].condition == 0) {
							if(incidentObject.name == "SpeedUpBoards") {
								userCondition = 2;
							}
							UpdateUserCondition(targetObjectName, userCondition);
							return 1;
						}
						else if(cars[targetObjectName].condition == 2) {
							if(incidentObject.name == "SpeedUpBoards") {
								return -1;
							}
							UpdateUserCondition(targetObjectName, userCondition);
							return 1;
						}
					}
				}
				return 0;
			}
			else {
				Debug.LogWarning("The system cannot find the target:" + targetObjectName);
			}
		}
		else if(incidentObject.tag == targetObject.tag) {
			return 0;
		}
		return -1;
	}

	/// <summary>Update the condition.</summary>
	/// <param name="carName">The name for User</param>
	/// <param name="carCondition">The condition of each user</param>
	public void UpdateUserCondition(string carName, int carCondition) {
		if(cars.ContainsKey(carName)) {
			cars[carName].condition = carCondition;
			switch(carCondition) {
				case 1:
					StartCoroutine(ViewerController.instance.ChangeDamageView(cars[carName].camera));
					break;
				default:
					break;
			}
		}
		else {
			Debug.LogWarning("The system cannot find the target:" + carName);
		}
	}

}
