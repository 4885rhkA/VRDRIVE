using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Class for the dropping the stage
public class UnderGround : Incident {

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, true}, 	// OnTriggerEnter
			{false, true}, 	// OnCollisionEnter
			{false, false}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollisionActionForMyself() {
	}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	protected override void CollisionActionForUser(string userName) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (userState.Status < 1) {
			GameController.instance.UpdateRecord (userObject.Obj.name, TimerController.instance.PastTime);
			GameController.instance.UpdateUserStatus(userObject.Obj.name, 2);
			GameController.instance.MissGame(userObject.Obj.name);
		}
	}


}
