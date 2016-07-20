using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	public Vector3 localGravity;

	/// <summary>remove the status</summary>
	/// <param name="rigid">target</param>
	public void RemoveDefaultGravity(Rigidbody rigid) {
		rigid.useGravity = false;
	}

	/// <summary>add force</summary>
	public void AddLocalGravity(Rigidbody rigid){
		rigid.AddForce(localGravity, ForceMode.Acceleration);
	}

	/// <summary>release freezing</summary>
	public void ReleaseFreezing(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.None;
	}

	/// <summary>need to freeze when start</summary>
	public void SetFreezing(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

}
