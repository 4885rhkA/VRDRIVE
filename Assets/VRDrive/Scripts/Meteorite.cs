using UnityEngine;
using System.Collections;

public class Meteorite : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {
		Detonator detonator = gameObject.GetComponent<Detonator>();
		float clipLength = SoundController.instance.GetClipLength("meteoriteexplosion");
		Destroy(gameObject.transform.FindChild("ShapeMeteorite").gameObject);
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
		gameObject.GetComponent<SphereCollider>().enabled = false;
		detonator.duration = clipLength;
		detonator.destroyTime = clipLength + 1;
		gameObject.GetComponent<AudioSource>().enabled = false;
		SoundController.instance.ShotClipSound("meteoriteexplosion");
		gameObject.GetComponent<Detonator>().Explode();
	}

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {}

	/// <summary>After collider occurs, do  action.</summary>
	protected override void AfterTriggerEnterAction(Collider collider) {}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {
		StartCoroutine(AfterCollisionEnter(
			SoundController.instance.GetClipLength("meteoriteexplosion"), 
			GameController.cars[collision.gameObject.name].obj.name, 0, collision));
	}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {
		GameController.instance.UpdateUserCondition(GameController.cars[collision.gameObject.name].obj.name, 0);
	}

}
