using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Max speed trigger.
/// </summary>
public class MaxSpeedTrigger: Incident {

	private float maxSpeed;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, false }, 	// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, true }, 	// OnTriggerStay
			{ false, false },	// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }	// OnCollisionExit
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
		maxSpeed = float.Parse(parentName.Replace("kmh", ""));
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

		if(ContainedCheckList()) {
			if(kindOfCollision == 2) {
				if(GameController.instance.IsPlayer(userObject.Obj.name)) {
					StartCoroutine(SetScreenshots(userObject.Obj.name));
				}
				if(userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed() > maxSpeed && userState.CheckList[parentName]) {
					GameController.instance.UpdateCheckList(userObject.Obj.name, parentName, false);
				}
			}
		}
	}

}
