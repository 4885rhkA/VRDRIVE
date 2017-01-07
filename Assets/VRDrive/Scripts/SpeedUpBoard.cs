using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

/// Class for defined action when collision between user's car and <c>SpeedUpBoard</c>
public class SpeedUpBoard : Incident {

	private float pushPower = 100;
	private float multipleSpeed = 3;
	private float blurAmount = 0.3f;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, true}, 	// OnTriggerEnter
			{false, true}, 	// OnCollisionEnter
			{false, false}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	void Start() {
		iTween.RotateTo (gameObject.transform.FindChild("SpeedUpBoardMessage/SpeedUpBoardTriangle").gameObject, iTween.Hash (
			"y", 360, "time", 0.5, "islocal", true, "loopType", "loop", "easeType", "linear"
		));
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollisionActionForMyself() {}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	protected override void CollisionActionForUser(string userName) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (userState.Condition == 0) {
			GameController.instance.UpdateUserCondition (userObject.Obj.name, 2);
			ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, blurAmount);
			userObject.Obj.GetComponent<MyCarController>().MaxSpeed *= multipleSpeed;
			userObject.Obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower, ForceMode.VelocityChange);
			SoundController.instance.ShotClipSound("speedup");
			GameController.instance.UpdateUserStatus(userObject.Obj.name, 0);
			StartCoroutine(AfterCollisionAction(SoundController.instance.GetClipLength("speedup"), userSet));
		}
	}

	/// <summary>After collider/collision occurs, do action.</summary>
	/// <param name="userSet">User's State and Object</param>
	private IEnumerator AfterCollisionAction(float delay, UserSet userSet) {
		yield return new WaitForSeconds(delay);

		UserObject userObject = userSet.UserObject;

		GameController.instance.UpdateUserCondition(userObject.Obj.name, 0);
		ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, 0);

		// TODO fix this function
		userObject.Obj.GetComponent<MyCarController>().MaxSpeed /= multipleSpeed;
	}

}
