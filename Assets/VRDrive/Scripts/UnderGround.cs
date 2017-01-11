using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Class for the dropping the stage
public class UnderGround : Incident {

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, true }, 		// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, false }, 	// OnTriggerStay
			{ false, false },		// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }		// OnCollisionExit
		};
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;

		GameController.instance.MissGame(userObject.Obj.name);
	}

}
