using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class MaxSpeed: Incident {

	private float maxSpeed;
	private string parentName;

	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
		maxSpeed = float.Parse (parentName.Replace ("km", ""));
	}


	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollidedActionForMyself() {
	}

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		if (userState.obj.GetComponent<MyCarController> ().GetCurrentSpeed () > maxSpeed && userState.checks[parentName] == true) {
			GameController.instance.SetUserCheck (collider.gameObject.name, parentName, false);
		}
	}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
	}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {
	}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {
	}
}
