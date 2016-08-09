using UnityEngine;
using System;

/// Class of the values for oprating parameters.
public class UserState {

	/// <list type="bullet">
	/// 	<item>
	/// 		<term>status</term>
	/// 		<description>By its value, controll the user.</description>
	/// 		<value>-1:standby / 0:nowplaying / 1:goal / 2:retire</value>
	/// 	</item>
	/// 	<item>
	/// 		<term>condition</term>
	/// 		<description>By its value, decide whether collision occurs or not.</description>
	/// 		<value>-1:courseout / 0:normal / 1:damage / 2:dash</value>
	/// 	</item>
	/// 	<item>
	/// 		<term>obj</term>
	/// 		<description>The <c>GameObject</c> for user's car</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>rigid</term>
	/// 		<description>The <c>Rigidbody</c> for user's car</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>camera</term>
	/// 		<description>The <c>GameObject</c> for user's camera</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>timer</term>
	/// 		<description>The <c>GameObject</c> for user's canvas of timer</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>message</term>
	/// 		<description>The <c>GameObject</c> for user's canvas of message</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>result</term>
	/// 		<description>The <c>GameObject</c> for user's canvas of result</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>record</term>
	/// 		<description>The goal of missed record</description>
	/// 	</item>
	/// </list>
	public int status;
	public int condition;
	public GameObject obj;
	public Rigidbody rigid;
	public GameObject camera;
	public GameObject timer;
	public GameObject message;
	public GameObject result;
	public TimeSpan record;
	public UserState(GameObject carObject) {
		status = -1;
		condition = 0;
		obj = carObject;
		rigid = carObject.GetComponent<Rigidbody>();
		if(obj.transform.FindChild("MainCamera" + carObject.name[carObject.name.Length - 1])) {
			camera = obj.transform.FindChild("MainCamera" + carObject.name[carObject.name.Length - 1]).gameObject;
		}
		else {
			camera = GameObject.Find("MainCamera" + carObject.name[carObject.name.Length - 1]).gameObject;
		}
		timer = carObject.transform.FindChild("Canvas/Timer").gameObject;
		message = carObject.transform.FindChild("Canvas/Message").gameObject;
		result = carObject.transform.FindChild("Canvas/Result").gameObject;
		record = new TimeSpan(0, 0, 0);
	}

}
