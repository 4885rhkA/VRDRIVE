using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	public static UserController instance;

	[SerializeField] private Vector3 localGravity;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	/// <summary>Remove the setting of default gravity.</summary>
	/// <param name="rigid">The target's Rigidbody Component</param>
	public void RemoveDefaultGravity(Rigidbody rigid) {
		rigid.useGravity = false;
	}

	/// <summary>Add force.</summary>
	/// <param name="rigid">The target's Rigidbody Component</param>
	public void AddLocalGravity(Rigidbody rigid){
		rigid.AddForce(localGravity, ForceMode.Acceleration);
	}

	/// <summary>Release freezing.</summary>
	/// <param name="rigid">The target's Rigidbody Component</param>
	public void ReleaseFreezing(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.None;
	}

	/// <summary>Freeze for the time before starting.</summary>
	/// <param name="rigid">The target's Rigidbody Component</param>
	public void SetFreezing(Rigidbody rigid) {
		rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

}
