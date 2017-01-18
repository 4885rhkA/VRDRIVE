using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Incident.
/// </summary>
public abstract class Incident : MonoBehaviour {

	protected bool[, ] collisionFlag;
	protected string parentName;

	protected bool nowTakingScreenshot = false;

	void OnTriggerEnter(Collider collider) {
		CollisionAction(collisionFlag[0, 0], collisionFlag[0, 1], collider.gameObject, 0);
	}

	void OnCollisionEnter(Collision collision) {
		CollisionAction(collisionFlag[1, 0], collisionFlag[1, 1], collision.gameObject, 1);
	}

	void OnTriggerStay(Collider collider) {
		CollisionAction(collisionFlag[2, 0], collisionFlag[2, 1], collider.gameObject, 2);
	}

	void OnCollisionStay(Collision collision) {
		CollisionAction(collisionFlag[3, 0], collisionFlag[3, 1], collision.gameObject, 3);
	}

	void OnTriggerExit(Collider collider) {
		CollisionAction(collisionFlag[4, 0], collisionFlag[4, 1], collider.gameObject, 4);
	}

	void OnCollisionExit(Collision collision) {
		CollisionAction(collisionFlag[5, 0], collisionFlag[5, 1], collision.gameObject, 5);
	}

	/// <summary>
	/// Collisions the action.
	/// </summary>
	/// <param name="myselfFlag">If set to <c>true</c> myself flag.</param>
	/// <param name="userFlag">If set to <c>true</c> user flag.</param>
	/// <param name="collidedObject">Collided object.</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected void CollisionAction(bool myselfFlag, bool userFlag, GameObject collidedObject, int kindOfCollision) {
		if(GameController.instance.HasUserSet(collidedObject.name)) {
			UserState userState = GameController.instance.GetUserSet(collidedObject.name).UserState;
			if(myselfFlag) {
				CollisionActionForMyself(kindOfCollision);
			}
			if(userFlag && userState.Status < 1) {
				CollisionActionForUser(collidedObject.name, kindOfCollision);
			}
		}
		else if(collidedObject.tag != "Untagged" && gameObject.tag == collidedObject.tag) {
			CollisionActionForMyself(kindOfCollision);
		}
	}

	/// <summary>
	/// Containeds the check list.
	/// </summary>
	/// <returns><c>true</c>, if check list was containeded, <c>false</c> otherwise.</returns>
	protected bool ContainedCheckList() {
		return gameObject.transform.root.gameObject.name == "CheckList";
	}

	/// <summary>
	/// Sets the screenshots.
	/// </summary>
	/// <returns>The screenshots.</returns>
	/// <param name="playerName">Player name.</param>
	protected IEnumerator SetScreenshots(string playerName) {
		if(!nowTakingScreenshot) {
			nowTakingScreenshot = true;
			string key = CameraController.instance.CreateKeyForScreenshot(playerName, parentName);
			yield return StartCoroutine(CameraController.instance.CaptureAndSaveScreenshots(key));
			yield return new WaitForSeconds(CameraController.instance.Intereval);
			nowTakingScreenshot = false;
		}
	}

	/// <summary>
	/// Collisions the action for myself.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected abstract void CollisionActionForMyself(int kindOfCollision);

	/// <summary>
	/// Collisions the action for user.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected abstract void CollisionActionForUser(string userName, int kindOfCollision);

}
