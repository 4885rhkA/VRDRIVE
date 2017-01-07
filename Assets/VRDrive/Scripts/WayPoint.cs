using UnityEngine;
using System.Collections;

public class WayPoint : Incident {

	Transform startWaypointTransform;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, true}, 	// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
			{false, false}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	void Start() {
		startWaypointTransform = transform.parent.Find ("Waypoint 000").gameObject.transform;
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollisionActionForMyself(int kindOfCollision) {}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;

		if(GameController.instance.isPlayer(userObject.Obj.name)) {
			userObject.Obj.transform.position = startWaypointTransform.position;
			userObject.Obj.transform.rotation = startWaypointTransform.rotation;
			userObject.Obj.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		}
	}
}
