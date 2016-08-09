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
		UserState userState = GameController.cars[collider.gameObject.name];
		userState.record = TimerController.instance.pastTime;
		StartCoroutine(AfterTriggerEnter(0, userState.obj.name, 2, collider));
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
