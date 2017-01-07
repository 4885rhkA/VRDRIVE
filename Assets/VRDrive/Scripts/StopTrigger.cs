using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class StopTrigger : Incident {

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, false}, 	// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
			{false, true}, 	// OnTriggerStay
			{false, true},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
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

		if (userObject.Obj.GetComponent<MyCarController> ().GetCurrentSpeed () < 5 && userState.CheckList[parentName] == false) {
			GameController.instance.UpdateCheckList (GetComponent<Collider>().gameObject.name, parentName, true);
		}
	}

}
