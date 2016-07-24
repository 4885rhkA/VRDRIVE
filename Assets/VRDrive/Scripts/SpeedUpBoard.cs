using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class SpeedUpBoard : Incident {

	private float keepSec = 1;
	private float multipleSpeed = 2;
	private float blurAmount = 0.3f;

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		GameObject userObj = collider.gameObject;
		ViewerController.instance.ChangeMotionBlur(userObj.transform.FindChild("MainCamera").gameObject, blurAmount);
		SoundController.instance.ShotSpeedUpSound();
		userObj.GetComponent<CarController>().MaxSpeed *= multipleSpeed;
		userObj.GetComponent<Rigidbody>().AddForce(Vector3.forward * 200, ForceMode.VelocityChange);
		StartCoroutine(AfterTriggerEnter(keepSec, userObj.name, 0, collider));
	}

	/// <summary>After collider occurs, do  action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		GameObject userObj = collider.gameObject;
		ViewerController.instance.ChangeMotionBlur(userObj.transform.FindChild("MainCamera").gameObject, 0);
		userObj.GetComponent<CarController>().MaxSpeed /= multipleSpeed;
	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}


}
