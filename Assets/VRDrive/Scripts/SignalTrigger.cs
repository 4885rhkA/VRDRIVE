using UnityEngine;
using System.Collections;

public class SignalTrigger : Incident {

	private Signal signal;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, false}, 	// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
			{false, false}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, true}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	void Start() {
		GameObject parentObj = gameObject.transform.parent.gameObject;
		signal = parentObj.GetComponent<Signal> ();
		parentName = parentObj.name;
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

		if(signal.GetStatus() == 2 && userState.CheckList[parentName]) {
			GameController.instance.UpdateCheckList (userObject.Obj.name, parentName, false);
		}
	}

}
