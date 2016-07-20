using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>This script operate the game progress. We can oprate about the time and the status</summary>
public class GameController : MonoBehaviour {
	
	/// <summary>This class contains the values for oprating parameters</summary>
	/// <param name="status"> -1:standby, 0:nowplaying, 1:goal, 2:retire</param>
	/// <param name="rank">The first rank is 0, not 1</param>
	/// <param name="record">The time for goal</param>
	/// <param name="obj">Gameobject of the car</param>

	public class CarStatus {
		public int status;
		public int rank;
		public TimeSpan record;
		public GameObject obj;
		public Rigidbody rigid;
		public Text timerText;
		public CarStatus(GameObject carObject) {
			status = -1;
			rank = 0;
			record = new TimeSpan(0, 0, 0);
			obj = carObject;
			rigid = null;
			timerText = null;
		}
	}

	private Dictionary<string, CarStatus> cars = new Dictionary<string, CarStatus>();
	private GameObject[] carObjects;

	private TimerController timerController;
	private ViewerController viewerController;
	private UserController userController;

	TimeSpan timeSpan = new TimeSpan(0, 0, 0);

	/// <summary>initialize when start</summary>
	void Start() {
		timerController = gameObject.GetComponent<TimerController>();
		viewerController = gameObject.GetComponent<ViewerController>();
		userController = gameObject.GetComponent<UserController>();
		carObjects = GameObject.FindGameObjectsWithTag("Car");
		if(carObjects != null) {
			foreach(GameObject carObject in carObjects) {
				cars.Add(carObject.name, new CarStatus(carObject));
			}
			carObjects = null;
			foreach(KeyValuePair<string, CarStatus> car in cars){
				car.Value.rigid = car.Value.obj.GetComponent<Rigidbody>();
				car.Value.timerText = car.Value.obj.FindDeep("TimerText").GetComponent<Text>();
				userController.SetFreezing(car.Value.rigid);
				viewerController.ChangeTimerState(car.Value.timerText, false);
				userController.RemoveDefaultGravity(car.Value.rigid);
			}
		}
	}

	void Update() {
		timeSpan = timerController.getPastTime();
		foreach(KeyValuePair<string, CarStatus> car in cars){
			viewerController.SetTimerTextToView(car.Value.timerText, timeSpan);
			userController.AddLocalGravity(car.Value.rigid);
		}
	}

	/// <summary>set the status for all users</summary>
	/// <param name="status"> -1:standby, 0:nowplaying, 1:goal, 2:retire</param>
	public void ChangeAllStatus(int status) {
		foreach(KeyValuePair<string, CarStatus> car in cars){
			ChangeStatus(status, car.Value.obj.name);
		}
	}

	/// <summary>set the status</summary>
	/// <param name="status"> -1:standby, 0:nowplaying, 1:goal, 2:retire</param>
	public void ChangeStatus(int status, String carName) {
		switch(status) {
			case -1:
				if(cars.ContainsKey(carName)) {
					userController.SetFreezing(cars[carName].rigid);
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
			case 0:
				if(cars.ContainsKey(carName)) {
					userController.ReleaseFreezing(cars[carName].rigid);
					cars[carName].status = 0;
					viewerController.ChangeTimerState(cars[carName].timerText, true);
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
			case 1:
				if(cars.ContainsKey(carName)) {
					cars[carName].record = gameObject.GetComponent<TimerController>().getPastTime();
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
		}
	}
}
