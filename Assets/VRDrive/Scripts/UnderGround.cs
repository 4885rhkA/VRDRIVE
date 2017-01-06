using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Class for the dropping the stage
public class UnderGround : Incident {

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserSet userSet = GameController.instance.GetUserSet (collider.gameObject.name);
		UserObject userObject = userSet.UserObject;

		GameController.instance.UpdateRecord (userObject.Obj.name, TimerController.instance.PastTime);
		StartCoroutine(AfterTriggerEnter(0, userObject.Obj.name, 2, collider));
	}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		GameController.instance.MissGame(collider.gameObject.name);
	}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
