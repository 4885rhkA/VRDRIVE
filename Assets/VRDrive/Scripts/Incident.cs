using UnityEngine;
using System.IO;
using System.Collections;

/// The Class for defined action when collision between user's car and gimmick
public abstract class Incident : MonoBehaviour {

	protected bool[, ] collisionFlag;
	protected string parentName;

	protected bool isSituationForSaveScreenshot = true;

	public bool IsSituationForSaveScreenshot {
		get {
			return isSituationForSaveScreenshot;
		}
		set {
			isSituationForSaveScreenshot = value;
		}
	}

	void OnTriggerEnter(Collider collider) {
		CollisionAction (collisionFlag[0, 0], collisionFlag[0, 1], collider.gameObject, 0);
	}

	void OnCollisionEnter(Collision collision) {
		CollisionAction (collisionFlag[1, 0], collisionFlag[1, 1], collision.gameObject, 1);
	}

	void OnTriggerStay(Collider collider) {
		CollisionAction (collisionFlag[2, 0], collisionFlag[2, 1], collider.gameObject, 2);
	}

	void OnCollisionStay(Collision collision) {
		CollisionAction (collisionFlag[3, 0], collisionFlag[3, 1], collision.gameObject, 3);
	}

	void OnTriggerExit(Collider collider) {
		CollisionAction (collisionFlag[4, 0], collisionFlag[4, 1], collider.gameObject, 4);
	}

	void OnCollisionExit(Collision collision) {
		CollisionAction (collisionFlag[5, 0], collisionFlag[5, 1], collision.gameObject, 5);
	}

	protected void CollisionAction(bool myselfFlag, bool userFlag, GameObject collidedObject, int kindOfCollision) {
		if(GameController.instance.HasUserSet(collidedObject.name)) {
			UserState userState = GameController.instance.GetUserSet (collidedObject.name).UserState;
			if(myselfFlag) {
				CollisionActionForMyself(kindOfCollision);
			}
			if(userFlag && userState.Status < 1) {
				CollisionActionForUser(collidedObject.name, kindOfCollision);
			}
		}
		else if(collidedObject.tag!= "Untagged" && gameObject.tag == collidedObject.tag) {
			CollisionActionForMyself(kindOfCollision);
		}
	}

	protected bool ContainedCheckList() {
		return gameObject.transform.root.gameObject.name == "CheckList";
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected abstract void CollisionActionForMyself(int kindOfCollision);

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	/// <param name="kindOfCollision">
	/// 	The kind of collision
	/// 	<value>0:OnTriggerEnter / 1:OnCollisionEnter / 2:OnTriggerStay / 3:OnCollisionStay / 4:OnTriggerExit / 5:OnCollisionExit</value>
	/// </param>
	protected abstract void CollisionActionForUser(string userName, int kindOfCollision);

	protected IEnumerator IsSituationForSaveScreenshotWithDelay(bool value) {
		yield return new WaitForSeconds(CameraController.instance.DelayForCaptureAfterTriggerExit);
		IsSituationForSaveScreenshot = value;
	}

	protected IEnumerator SaveScreenshotWithInterval(string userName) {
		int count = 0;
		CameraController cameraController = CameraController.instance;
		string directoryForScreenshot = cameraController.DirectoryForScreenshot;
		float intervalForCapture = cameraController.IntervalForCapture;
		string path = directoryForScreenshot + "/" + parentName;
		Directory.CreateDirectory(Application.dataPath + "/" + path);

		path = "Assets/" + path;
		while (isSituationForSaveScreenshot) {
			Application.CaptureScreenshot (path + "/" + userName + "-" + count.ToString().PadLeft(4, '0') +".png");
			count++;
			yield return new WaitForSeconds(intervalForCapture);
		}
	}

}
