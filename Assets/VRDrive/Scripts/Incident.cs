using UnityEngine;
using System.Collections;

/// The Class for defined action when collision between user's car and gimmick
public abstract class Incident : MonoBehaviour {

	protected bool[, ] collisionFlag;
	protected string parentName;

	void OnTriggerEnter(Collider collider) {
		CollisionAction (collisionFlag[0, 0], collisionFlag[0, 1], collider.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		CollisionAction (collisionFlag[1, 0], collisionFlag[1, 1], collision.gameObject);
	}

	void OnTriggerStay(Collider collider) {
		CollisionAction (collisionFlag[2, 0], collisionFlag[2, 1], collider.gameObject);
	}

	void OnCollisionStay(Collision collision) {
		CollisionAction (collisionFlag[3, 0], collisionFlag[3, 1], collision.gameObject);
	}

	void OnTriggerExit(Collider collider) {
		CollisionAction (collisionFlag[4, 0], collisionFlag[4, 1], collider.gameObject);
	}

	void OnCollisionExit(Collision collision) {
		CollisionAction (collisionFlag[5, 0], collisionFlag[5, 1], collision.gameObject);
	}

	protected void CollisionAction(bool myselfFlag, bool userFlag, GameObject collidedObject) {
		if(GameController.instance.HasUserSet(collidedObject.name)) {
			if(myselfFlag) {
				CollisionActionForMyself();
			}
			if(userFlag) {
				CollisionActionForUser(collidedObject.name);
			}
		}
		else if(collidedObject.tag!= "Untagged" && gameObject.tag == collidedObject.tag) {
			CollisionActionForMyself();
		}
	}

	protected abstract void CollisionActionForMyself();

	protected abstract void CollisionActionForUser(string collidedObjectName);

}
