using UnityEngine;
using System.Collections;

/// Class of the values for caching.
public class UserObject {

	/// <list type="bullet">
	/// 	<item>
	/// 		<term>obj</term>
	/// 		<description>The <c>GameObject</c> for user's car</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>mainCamera</term>
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
	/// 	<item>
	/// 		<term>howTo</term>
	/// 		<description>The <c>GameObject</c> for user's canvas of howto</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>speedMetor</term>
	/// 		<description>The showing of the speed. Moreover, only contained in Car</description>
	/// 	</item>
	/// </list>

	private GameObject obj;
	private GameObject mainCamera;
	private GameObject timer;
	private GameObject message;
	private GameObject result;
	private GameObject howTo;
	private GameObject speedMeter;

	public UserObject() {
	}

	public UserObject(GameObject userObject) {
		obj = userObject;
		mainCamera = obj.transform.FindChild("MainCamera" + obj.name[obj.name.Length - 1]).gameObject;
		timer = obj.transform.FindChild("Canvas/Timer").gameObject;
		message = obj.transform.FindChild("Canvas/Message").gameObject;
		result = obj.transform.FindChild("Canvas/Result").gameObject;
		howTo = obj.transform.FindChild("Canvas/HowTo").gameObject;

		if (obj.transform.FindChild ("Meter/Speed") != null) {
			speedMeter = obj.transform.FindChild ("Meter/Speed").gameObject;
		}
		else {
			speedMeter = null;
		}
	}

	public GameObject Obj {
		get {
			return obj;
		}
	}

	public GameObject MainCamera {
		get {
			return mainCamera;
		}
	}

	public GameObject Timer {
		get {
			return timer;
		}
	}

	public GameObject Message {
		get {
			return message;
		}
	}

	public GameObject Result {
		get {
			return result;
		}
	}

	public GameObject HowTo {
		get {
			return howTo;
		}
	}

	public GameObject SpeedMeter {
		get {
			return speedMeter;
		}
	}

}
