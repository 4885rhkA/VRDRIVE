using UnityEngine;
using System.Collections;

/// Control class for the each user's rigidbody
public class UserController : MonoBehaviour {

	public static UserController instance;

	[SerializeField] private Vector3 localGravity;

	void Awake() {
		instance = this;
	}

	/// <summary>Remove the setting of default gravity.</summary>
	/// <param name="rigid">The target's <c>Rigidbody</c> Component</param>
	public void RemoveDefaultGravity(Rigidbody rigid) {
		rigid.useGravity = false;
	}

	/// <summary>Add force.</summary>
	/// <param name="rigid">The target's <c>Rigidbody</c> Component</param>
	public void AddLocalGravity(Rigidbody rigid) {
		rigid.AddForce(localGravity, ForceMode.Acceleration);
	}

	/// <summary>Release freezing position.</summary>
	/// <param name="rigid">The target's <c>Rigidbody</c> Component</param>
	public void ReleaseFreezingPosition(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.None;
	}

	/// <summary>Freeze position for the time before starting.</summary>
	/// <param name="rigid">The target's <c>Rigidbody</c> Component</param>
	public void SetFreezingPosition(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

}
