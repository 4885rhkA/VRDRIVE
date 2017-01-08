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
			{false, true}, 		// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
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
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected override void CollisionActionForMyself(int kindOfCollision) {}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (userState.Condition == 0) {
			if(GameController.instance.isPlayer(userObject.Obj.name)) {
				ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, blurAmount);
				SoundController.instance.ShotClipSound("speedup");
			}			GameController.instance.UpdateUserCondition (userObject.Obj.name, 2);
			userObject.Obj.GetComponent<MyCarController>().MaxSpeed *= multipleSpeed;
			userObject.Obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * pushPower, ForceMode.VelocityChange);
			GameController.instance.UpdateUserStatus(userObject.Obj.name, 0);
			StartCoroutine(AfterCollisionAction(SoundController.instance.GetClipLength("speedup"), userSet));
		}
	}

	/// <summary>After collider/collision occurs, do action.</summary>
	/// <param name="delay">How long it occurs</param>
	/// <param name="userSet">User's State and Object</param>
	private IEnumerator AfterCollisionAction(float delay, UserSet userSet) {
		yield return new WaitForSeconds(delay);

		UserObject userObject = userSet.UserObject;

		if (GameController.instance.isPlayer (userObject.Obj.name)) {
			ViewerController.instance.ChangeMotionBlur(userObject.MainCamera, 0);
		}
		GameController.instance.UpdateUserCondition(userObject.Obj.name, 0);

		userObject.Obj.GetComponent<MyCarController>().MaxSpeed /= multipleSpeed;
	}

}
