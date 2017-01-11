using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Signal trigger.
/// </summary>
public class SignalTrigger : Incident {

	private Signal signal;
	private int brakePowerGivenByAI = 5;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, false }, 	// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, true }, 	// OnTriggerStay
			{ false, false },	// OnCollisionStay
			{ false, true }, 	// OnTriggerExit
			{ false, false }	// OnCollisionExit
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		GameObject parentObj = gameObject.transform.parent.gameObject;
		signal = parentObj.GetComponent<Signal>();
		parentName = parentObj.name;
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
