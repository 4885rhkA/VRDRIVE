using UnityEngine;
using System.Collections;

public class Meteorite : Incident {

	[SerializeField] private int attackPower = 200000;
	[SerializeField] private int[] moveTime = new int[2]{10, 11};
	private float[] moveToPosition;

	void Start() {
		if(moveTime[0] < moveTime[1]) {
			moveToPosition = new float[3]{ gameObject.transform.position.x, gameObject.transform.position.y, -20 };
			iTween.RotateAdd(gameObject.transform.FindChild("ShapeMeteorite").gameObject, iTween.Hash(
				"x", Random.Range(0, 2) * 360, "y", Random.Range(0, 2) * 360, "z", Random.Range(0, 2) * 360, 
				"time", Random.Range(10, 21), "easeType", "linear", "loopType", "loop"
			));
			iTween.MoveTo(gameObject, iTween.Hash(
				"x", moveToPosition[0], "y", moveToPosition[1], "z", moveToPosition[2], 
				"time", Random.Range(moveTime[0], moveTime[1]), "easeType", "linear"
			));
		}
		else {
			Debug.LogWarning("You need to set the correct number, the range of the moving time for the meteorite.");
		}
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
		userState.rigid.AddForce(new Vector3(direction.x, 0, Mathf.Abs(direction.z)) * attackPower * (-1), ForceMode.Impulse);
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
