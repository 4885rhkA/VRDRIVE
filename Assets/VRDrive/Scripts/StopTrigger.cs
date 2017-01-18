using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Stop trigger.
/// </summary>
public class StopTrigger : Incident {

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

	void Start() {
		parentName = gameObject.transform.parent.gameObject.name;
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
				if(userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed() < 5 && userState.CheckList[parentName] == false) {
					GameController.instance.UpdateCheckList(userObject.Obj.name, parentName, true);
				}
			}
			if(kindOfCollision == 4 && GameController.instance.WarningMode) {
				if(!GameController.instance.GetCheck(userObject.Obj.name, parentName)) {
					GameController.instance.ShowWarning(userObject.Obj.name, parentName);
				}
			}
		}
	}

}
