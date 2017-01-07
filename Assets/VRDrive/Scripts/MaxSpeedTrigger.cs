using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

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
		maxSpeed = float.Parse (parentName.Replace ("km", ""));
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (ContainedCheckList ()) {
			if (userObject.Obj.GetComponent<MyCarController> ().GetCurrentSpeed () > maxSpeed && userState.CheckList[parentName]) {
				GameController.instance.UpdateCheckList (userObject.Obj.name, parentName, false);
			}
		}
	}

}
