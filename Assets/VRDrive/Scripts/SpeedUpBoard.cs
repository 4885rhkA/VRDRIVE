using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// Class for defined action when collision between user's car and <c>SpeedUpBoard</c>
public class SpeedUpBoard : Incident {

	private float pushPower = 100;
	private float multipleSpeed = 3;
	private float blurAmount = 0.3f;

	void Start() {
		iTween.RotateTo (gameObject.transform.FindChild("SpeedUpBoardMessage/SpeedUpBoardTriangle").gameObject, iTween.Hash (
			"y", 360, "time", 0.5, "islocal", true, "loopType", "loop", "easeType", "linear"
		));
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject userObject = userState.obj;
		ViewerController.instance.ChangeMotionBlur(userState.camera, blurAmount);
		userObject.GetComponent<MyCarController>().MaxSpeed *= multipleSpeed;
		userObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower, ForceMode.VelocityChange);
		SoundController.instance.ShotClipSound("speedup");
		StartCoroutine(AfterTriggerEnter(SoundController.instance.GetClipLength("speedup"), 
			userObject.name, 0, collider));
	}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameController.instance.UpdateUserCondition(userState.obj.name, 0);
		ViewerController.instance.ChangeMotionBlur(userState.camera, 0);
		userState.obj.GetComponent<MyCarController>().MaxSpeed /= multipleSpeed;
	}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
