using UnityEngine;

/// <summary>
/// Start.
/// </summary>
public class Start : Incident {

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, false }, 	// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, false }, 	// OnTriggerStay
			{ false, false },	// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }	// OnCollisionExit
		};
	}

	/// <summary>
	/// Collisions the action for myself.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>
	/// Collisions the action for user.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
	}

}
