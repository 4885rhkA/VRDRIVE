using UnityEngine;
using System.Collections;

/// <summary>
/// User controller.
/// </summary>
public class UserController : MonoBehaviour {

	public static UserController instance;

	[SerializeField] private Vector3 localGravity;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
	}

	/// <summary>
	/// Removes the default gravity.
	/// </summary>
	/// <param name="rigid">Rigid.</param>
	public void RemoveDefaultGravity(Rigidbody rigid) {
		rigid.useGravity = false;
	}

	/// <summary>
	/// Adds the local gravity.
	/// </summary>
	/// <param name="rigid">Rigid.</param>
	public void AddLocalGravity(Rigidbody rigid) {
		rigid.AddForce(localGravity, ForceMode.Acceleration);
	}

	/// <summary>
	/// Releases the freezing position.
	/// </summary>
	/// <param name="rigid">Rigid.</param>
	public void ReleaseFreezingPosition(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.None;
	}

	/// <summary>
	/// Sets the freezing position.
	/// </summary>
	/// <param name="rigid">Rigid.</param>
	public void SetFreezingPosition(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

}
