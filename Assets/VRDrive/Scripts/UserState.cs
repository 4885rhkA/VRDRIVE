using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>This class contains the values for oprating parameters.</summary>
/// <param name="status">The status of each user(-1:standby, 0:nowplaying, 1:goal, 2:retire)</param>
/// <param name="consition">The condition of each user(-1:courseout, 0:normal, 1:damage, 2:dash)</param>
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
	public GameObject camera;
	public Text timerText;
	public GameObject message;
	public UserState(GameObject carObject) {
		status = -1;
		condition = 0;
		record = new TimeSpan(0, 0, 0);
		obj = carObject;
		rigid = carObject.GetComponent<Rigidbody>();
		camera = carObject.transform.FindChild("MainCamera").gameObject;
		timerText = carObject.transform.FindChild("Canvas/Timer/TimerText").gameObject.GetComponent<Text>();
		message = carObject.transform.FindChild("Canvas/Message").gameObject;
	}
}