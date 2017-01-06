using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class StopTrigger : Incident {

	private string parentName;

	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
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

		if (userObject.Obj.GetComponent<MyCarController> ().GetCurrentSpeed () < 5 && userState.CheckList[parentName] == false) {
			GameController.instance.UpdateCheckList (collider.gameObject.name, parentName, true);
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
