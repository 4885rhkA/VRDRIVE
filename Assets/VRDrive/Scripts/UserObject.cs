using UnityEngine;
using System.Collections;

/// <summary>
/// User object.
/// </summary>
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
	/// 		<term>image</term>
	/// 		<description>The <c>GameObject</c> for user's canvas of image</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>speedMetor</term>
	/// 		<description>The showing of the speed. Moreover, only contained in Car</description>
	/// 	</item>
	/// </list>

	private GameObject obj = null;
	private GameObject mainCamera = null;
	private GameObject timer = null;
	private GameObject message = null;
	private GameObject result = null;
	private GameObject image = null;
	private GameObject speedMeter = null;

	public UserObject() {
	}

	public UserObject(GameObject userObject) {
		obj = userObject;

		if(userObject.name.Contains("Player")) {
			mainCamera = obj.transform.FindChild("MainCamera" + obj.name[obj.name.Length - 1]).gameObject;
			timer = obj.transform.FindChild("Canvas/Timer").gameObject;
			message = obj.transform.FindChild("Canvas/Message").gameObject;
			result = obj.transform.FindChild("Canvas/Result").gameObject;
			image = obj.transform.FindChild("Canvas/Image").gameObject;
			if(obj.transform.FindChild("Meter/Speed") != null) {
				speedMeter = obj.transform.FindChild("Meter/Speed").gameObject;
			}
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

	public GameObject Image {
		get {
			return image;
		}
	}

	public GameObject SpeedMeter {
		get {
			return speedMeter;
		}
	}

}
