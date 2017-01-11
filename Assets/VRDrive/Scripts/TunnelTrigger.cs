using UnityEngine;
using System.Collections;

/// Class for defined action when collision between user's car and trigger of Tunnel
public class TunnelTrigger : Incident {

	private Transform startTransform;

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

	void Start() {
		startTransform = GameObject.Find("Start").gameObject.transform;
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

		if(GameController.instance.IsPlayer(userObject.Obj.name)) {
			StartCoroutine(ViewerController.instance.ChangeDamageView(userObject.MainCamera));
		}
		userObject.Obj.transform.position = startTransform.position;
		userObject.Obj.transform.rotation = startTransform.rotation;
		userObject.Obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}

}
