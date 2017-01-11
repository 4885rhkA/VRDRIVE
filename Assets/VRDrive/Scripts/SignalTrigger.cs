using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// Class for defined action when collision between user's car and trigger of Signal
public class SignalTrigger : Incident {

	private Signal signal;
	private int brakePowerGivenByAI = 5;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, false }, 		// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, true }, 		// OnTriggerStay
			{ false, false },		// OnCollisionStay
			{ false, true }, 		// OnTriggerExit
			{ false, false }		// OnCollisionExit
		};
	}

	void Start() {
		GameObject parentObj = gameObject.transform.parent.gameObject;
		signal = parentObj.GetComponent<Signal>();
		parentName = parentObj.name;
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
		UserState userState = userSet.UserState;


		if(kindOfCollision == 2) {
			if(signal.Status == 2 && !GameController.instance.IsPlayer(userObject.Obj.name)) {
				if(userObject.Obj.GetComponent<Rigidbody>().velocity.magnitude > 1) {
					Vector3 brakeVec = -Time.deltaTime * brakePowerGivenByAI * userObject.Obj.GetComponent<Rigidbody>().velocity;
					userObject.Obj.GetComponent<Rigidbody>().velocity += brakeVec;
				}
				else {
					userObject.Obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
				}
			}
		}
		if(ContainedCheckList()) {
			if(kindOfCollision == 2) {
				if(GameController.instance.IsPlayer(userObject.Obj.name)) {
					StartCoroutine(SetScreenshots(userObject.Obj.name));
				}
			}
			if(kindOfCollision == 4) {
				if(signal.Status == 2 && userState.CheckList[parentName]) {
					GameController.instance.UpdateCheckList(userObject.Obj.name, parentName, false);
				}
			}
		}

	}

}
