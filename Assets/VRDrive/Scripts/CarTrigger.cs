using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Car trigger.
/// </summary>
public class CarTrigger : Incident {

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, true }, 	// OnTriggerEnter
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
		parentName = gameObject.transform.parent.gameObject.name;
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

		// Little moving
		GameController.instance.GetUserSet(parentName).UserObject.Obj.GetComponent<MyCarController>().MaxSpeed = 0.1f;

		if(ContainedCheckList()) {
			if(kindOfCollision == 0) {
				GameController.instance.UpdateCheckList(userObject.Obj.name, parentName, false);
			}
			if(kindOfCollision == 2) {
				if(GameController.instance.IsPlayer(userObject.Obj.name)) {
					StartCoroutine(SetScreenshots(userObject.Obj.name));
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
