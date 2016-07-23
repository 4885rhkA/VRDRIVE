using UnityEngine;
using System.Collections;

public class Meteorite : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {}

	/// <summary>After collider occurs, do  action.</summary>
	protected override void AfterTriggerEnterAction(Collider collider) {}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
