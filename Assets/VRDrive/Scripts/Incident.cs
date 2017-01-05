using UnityEngine;
using System.Collections;

/// The Class for defined action when collision between user's car and gimmick
public abstract class Incident : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		int result = GameController.instance.UpdateGameState(gameObject.transform.root.gameObject, collider.gameObject.transform.root.gameObject);
		if(result > -1) {
			CollidedActionForMyself();
		}
		if(result > 0) {
			ColliderActionForUser(collider);
		}
	}

	void OnCollisionEnter(Collision collision) {
		int result = GameController.instance.UpdateGameState(gameObject.transform.root.gameObject, collision.gameObject.transform.root.gameObject);
		if(result > -1) {
			CollidedActionForMyself();
		}
		if(result > 0) {
			CollisionActionForUser(collision);
		}
	}

	/// <summary>After collider occurs, do object's action.</summary>
	/// <param name="delayLength">The delay how long this function will execute</param>
	/// <param name="carName">User's car name</param>
	/// <param name="carStatus">User's car status</param>
	/// <param name="collider">Collided object</param>
	protected IEnumerator AfterTriggerEnter(float delayLength, string carName, int carStatus, Collider collider) {  
		GameController.instance.UpdateUserStatus(carName, carStatus);
		yield return new WaitForSeconds(delayLength);
		AfterTriggerEnterAction(collider);
	}

	/// <summary>After collision occurs, do object's action.</summary>
	/// <param name="delayLength">The delay how long this function will execute</param>
	/// <param name="carName">User's car name</param>
	/// <param name="carStatus">User's car status</param>
	/// <param name="collision">collided object</param>
	protected IEnumerator AfterCollisionEnter(float delayLength, string carName, int carStatus, Collision collision) {  
		GameController.instance.UpdateUserStatus(carName, carStatus);
		yield return new WaitForSeconds(delayLength);
		AfterCollisionEnterAction(collision);
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected abstract void CollidedActionForMyself();

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected abstract void ColliderActionForUser(Collider collider);

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected abstract void AfterTriggerEnterAction(Collider collider);

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected abstract void CollisionActionForUser(Collision collision);

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected abstract void AfterCollisionEnterAction(Collision collision);

}
