using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class SpeedUpBoard : Incident {

	private float pushPower = 100;
	private float multipleSpeed = 3;
	private float blurAmount = 0.3f;

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject userObject = userState.obj;
		ViewerController.instance.ChangeMotionBlur(userState.camera, blurAmount);
		userObject.GetComponent<CarController>().MaxSpeed *= multipleSpeed;
		userObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower, ForceMode.VelocityChange);
		SoundController.instance.ShotClipSound("speedup");
		StartCoroutine(AfterTriggerEnter(SoundController.instance.GetClipLength("speedup"), userObject.name, 0, collider));
	}

	/// <summary>After collider occurs, do  action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		ViewerController.instance.ChangeMotionBlur(userState.camera, 0);
		userState.obj.GetComponent<CarController>().MaxSpeed /= multipleSpeed;
	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
