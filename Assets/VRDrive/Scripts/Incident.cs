using UnityEngine;
using System.Collections;

public abstract class Incident : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		int result = GameController.instance.UpdateGameState(gameObject.transform.root.gameObject, collider.gameObject.transform.root.gameObject);
		if(result > 0) {
			ColliderActionForUser(collider);
		}
		if(result > -1) {
			ColliderActionForMyself();
		}
	}

	void OnCollisionEnter(Collision collision) {
		int result = GameController.instance.UpdateGameState(gameObject.transform.root.gameObject, collision.gameObject.transform.root.gameObject);
		if(result > 0) {
			CollisionActionForUser(collision);
		}
		if(result > -1) {
			CollisionActionForMyself();
		}
	}

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected abstract void ColliderActionForUser(Collider collider);

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected abstract void CollisionActionForUser(Collision collision);

	/// <summary>When collider occurs, do Object's action.</summary>
	protected abstract void ColliderActionForMyself();

	/// <summary>When collision occurs, do Object's action.</summary>
	protected abstract void CollisionActionForMyself();

}
