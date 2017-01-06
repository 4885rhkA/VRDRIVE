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
		UserSet userSet = GameController.instance.GetUserSet (collider.gameObject.name);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;
		if (userObject.Obj.GetComponent<MyCarController> ().GetCurrentSpeed () > maxSpeed && userState.CheckList[parentName] == true) {
			GameController.instance.UpdateCheckList (userObject.Obj.name, parentName, false);
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
