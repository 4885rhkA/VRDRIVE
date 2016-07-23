using UnityEngine;
using System.Collections;

public class SpeedBoard : Incident {

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		Debug.Log("collider!!" + collider.gameObject.transform.root.gameObject.name);
	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {
		Debug.Log("collision!!" + collision.gameObject.transform.root.gameObject.name);
	}

	/// <summary>When collider occurs, do Object's action.</summary>
	protected override void ColliderActionForMyself() {
		Debug.Log("collider" + gameObject.name);
	}

	/// <summary>When collision occurs, do Object's action.</summary>
	protected override void CollisionActionForMyself() {
		Debug.Log("collision" + gameObject.name);
	}

	void OnTriggerStay(Collider collider) {
		collider.gameObject.transform.root.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10000, ForceMode.Force);
	}

	void OnTriggerExit(Collider collider) {
		ColliderAction(collider);
	}


}
