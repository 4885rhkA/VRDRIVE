using UnityEngine;
using System.Collections;

/// <summary>
/// Final way point.
/// </summary>
public class FinalWayPoint : Incident {

	Transform startWaypointTransform;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, true }, 	// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, false }, 	// OnTriggerStay
			{ false, false },	// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }	// OnCollisionExit
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		startWaypointTransform = transform.parent.Find("Waypoint 000").gameObject.transform;
	}

	/// <summary>
	/// When collider/collision occurs, do object's action.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>
	/// When collider/collision occurs, do user's action.
	/// </summary>
	/// <param name="userName">The name for user</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;

		if(!GameController.instance.IsPlayer(userObject.Obj.name)) {
			userObject.Obj.transform.position = startWaypointTransform.position;
			userObject.Obj.transform.rotation = startWaypointTransform.rotation;
			userObject.Obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		}
	}
}
