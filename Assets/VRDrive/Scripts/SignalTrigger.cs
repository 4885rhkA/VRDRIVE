using UnityEngine;
using System.Collections;

public class SignalTrigger : Incident {

	private Signal signal;
	private string parentName;

	void Start() {
		GameObject parentObj = gameObject.transform.parent.gameObject;
		signal = parentObj.GetComponent<Signal> ();
		parentName = parentObj.name;
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

		if(signal.GetStatus() == 2 && userState.CheckList[parentName] == true) {
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
