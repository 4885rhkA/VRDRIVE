using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
		public CarStatus(GameObject carObject) {
			status = -1;
			rank = 0;
			record = new TimeSpan(0, 0, 0);
			obj = carObject;
		}
	}

	private Dictionary<string, CarStatus> cars = new Dictionary<string, CarStatus>();
	private GameObject[] carObjects;

	private DateTime startTime;
	private TimeSpan pastTime;

	private AudioSource source;
	[SerializeField] private AudioClip countClip;
	[SerializeField] private AudioClip goClip;
	[SerializeField] private AudioClip backGroundClip;

	/// <summary>When start, first initialize the status, finally start sound and timer.</summary>
	void Start() {
		startTime = DateTime.Now;
		carObjects = GameObject.FindGameObjectsWithTag("Car");
		foreach(GameObject carObject in carObjects) {
			cars.Add(carObject.name, new CarStatus(carObject));
		}
		carObjects = null;
		source = gameObject.GetComponent<AudioSource>();
		source.clip = countClip;
		source.Play();
		StartCoroutine(StartGame(countClip.length));
	}

	private IEnumerator StartGame(float clipLength) {
		yield return new WaitForSeconds(clipLength);
		source.clip = backGroundClip;
		source.PlayOneShot(goClip);
		source.Play();
		startTime = DateTime.Now;
		ChangeStatus(0, null);
	}

	/// <summary>initialize the status</summary>
	/// <param name="status"> -1:standby, 0:nowplaying, 1:goal, 2:retire</param>
	void ChangeStatus(int status, String carName) {
		switch(status) {
			case -1:
				if(cars.ContainsKey(carName)) {
					cars[carName].obj.GetComponent<UserController>().SetFreezing();
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
			case 0:
				foreach(KeyValuePair<string, CarStatus> car in cars) {
				car.Value.obj.GetComponent<UserController>().ReleaseFreezing();
					car.Value.status = 0;
				}
				break;
			case 1:
				if(cars.ContainsKey(carName)) {
					cars[carName].rank = 1;
					cars[carName].record = pastTime;
				}
				else {
					Debug.LogWarning("The system cannot find the target:" + carName);
				}
				break;
		}
	}

	/// <summary>When update</summary>
	void Update() {
		TimerCount();
	}

	/// <summary>count the time</summary>
	void TimerCount() {
		pastTime = DateTime.Now - startTime;
	}
}
