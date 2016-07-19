using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	private Rigidbody rigidBody;

	void Start() {
		rigidBody = gameObject.GetComponent<Rigidbody>();
		SetFreezing();
	}

	public void ReleaseFreezing() {
		rigidBody.constraints = RigidbodyConstraints.None;
	}

	public void SetFreezing() {
		rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}
}
