using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Speed up board.
/// </summary>
public class SpeedUpBoard : Incident {

	private float pushPower = 100;
	private float multipleSpeed = 3;
	private float blurAmount = 0.3f;

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
		iTween.RotateTo(gameObject.transform.FindChild("SpeedUpBoardMessage/SpeedUpBoardTriangle").gameObject, iTween.Hash("y", 360, "time", 0.5, "islocal", true, "loopType", "loop", "easeType", "linear"));
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

		if(userState.Status < 1) {
			if(userState.Condition == 0) {
				if(GameController.instance.IsPlayer(userObject.Obj.name)) {
					ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, blurAmount);
					SoundController.instance.ShotClipSound("speedup");
				}
				GameController.instance.UpdateUserCondition(userObject.Obj.name, 2);
				userObject.Obj.GetComponent<MyCarController>().MaxSpeed *= multipleSpeed;
				userObject.Obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower, ForceMode.VelocityChange);
				StartCoroutine(AfterCollisionAction(SoundController.instance.GetClipLength("speedup"), userSet));
			}
		}
	}

	/// <summary>
	/// Afters the collision action.
	/// </summary>
	/// <returns>The collision action.</returns>
	/// <param name="delay">Delay.</param>
	/// <param name="userSet">User set.</param>
	private IEnumerator AfterCollisionAction(float delay, UserSet userSet) {
		yield return new WaitForSeconds(delay);

		UserObject userObject = userSet.UserObject;

		if(GameController.instance.IsPlayer(userObject.Obj.name)) {
			ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, 0);
		}
		GameController.instance.UpdateUserCondition(userObject.Obj.name, 0);

		userObject.Obj.GetComponent<MyCarController>().MaxSpeed /= multipleSpeed;
	}

}
