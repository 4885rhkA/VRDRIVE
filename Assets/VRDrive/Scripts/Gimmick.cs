using UnityEngine;
using System.Collections;

public abstract class Gimmick : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		GimmickController.instance.ColliderAction(gameObject, collider.gameObject.transform.root.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		GimmickController.instance.CollisionAction(gameObject, collision);
	}

	/// <summary>When collider occurs, do Object's action.</summary>
	protected abstract void ColliderAction();

	/// <summary>When collision occurs, do Object's action.</summary>
	/// <param name="collision">Attacked Object's collision</param>
	protected abstract void CollisionAction(Collision collision);

}
