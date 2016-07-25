using UnityEngine;
using System.Collections;

public class Meteorite : Incident {

	private int attackPower = 200000;

	void Awake() {
		iTween.RotateTo (gameObject.transform.FindChild("ShapeMeteorite").gameObject, iTween.Hash("x", 90, "y", 90, "z", 90, "time", 5, "islocal", true, "loopType", "loop"));
	}

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
		UserState userState = GameController.cars[collision.gameObject.name];
		Vector3 direction = userState.rigid.velocity.normalized;
		userState.rigid.AddForce(new Vector3(direction.x, 0, direction.z) * attackPower * (-1), ForceMode.Impulse);
		int carStatus = 0;
		if(GameController.instance.oneKillMode) {
			carStatus = 2;
		}
		StartCoroutine(AfterCollisionEnter(
			SoundController.instance.GetClipLength("meteoriteexplosion"), 
			userState.obj.name, carStatus, collision));
	}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {
		GameController.instance.UpdateUserCondition(GameController.cars[collision.gameObject.name].obj.name, 0);
	}

}
