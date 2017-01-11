using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// Class for defined action when collision between user's car and trigger of MaxSpeed Label
public class MaxSpeedTrigger: Incident {

	private float maxSpeed;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, false}, 	// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
			{false, true}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
		maxSpeed = float.Parse (parentName.Replace ("kmh", ""));
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
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (ContainedCheckList ()) {
			if (kindOfCollision == 2) {
				if(GameController.instance.IsPlayer(userObject.Obj.name)) {
					StartCoroutine (SetScreenshots (userObject.Obj.name));
				}
				if (userObject.Obj.GetComponent<MyCarController> ().GetCurrentSpeed () > maxSpeed && userState.CheckList[parentName]) {
					GameController.instance.UpdateCheckList (userObject.Obj.name, parentName, false);
				}
			}
		}
	}

}
