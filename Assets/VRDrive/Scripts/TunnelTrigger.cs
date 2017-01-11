using UnityEngine;
using System.Collections;

/// <summary>
/// Tunnel trigger.
/// </summary>
public class TunnelTrigger : Incident {

	private Transform startTransform;

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

	void Start() {
		startTransform = GameObject.Find("Start").gameObject.transform;
	}

	/// <summary>
	/// Collisions the action for myself.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>
	/// Collisions the action for user.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;

		if(GameController.instance.IsPlayer(userObject.Obj.name)) {
			StartCoroutine(ViewerController.instance.ChangeDamageView(userObject.MainCamera));
		}
		userObject.Obj.transform.position = startTransform.position;
		userObject.Obj.transform.rotation = startTransform.rotation;
		userObject.Obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}

}
