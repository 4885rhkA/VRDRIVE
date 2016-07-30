using UnityEngine;
using System.Collections;

/// Class for defined action when collision between user's car and meteorite
public class Meteorite : Incident {

	[SerializeField] private int attackPower = 200000;
	[SerializeField] private int[] moveTime = new int[2]{10, 11};
	private float[] moveToPosition;

	private AudioSource[] audioSources;
	private AudioSource loopSound;
	private AudioSource explosionSound;

	void Start() {
		audioSources = gameObject.GetComponents<AudioSource>();
		loopSound = audioSources[0];
		explosionSound = audioSources[1];
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

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollidedActionForMyself() {
		Detonator detonator = gameObject.GetComponent<Detonator>();
		float explosionLength = explosionSound.clip.length;
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
		gameObject.GetComponent<SphereCollider>().enabled = false;
		detonator.duration = explosionLength;
		detonator.destroyTime = explosionLength + 1;
		loopSound.enabled = false;
		explosionSound.enabled = true;
		explosionSound.PlayOneShot(explosionSound.clip);
		Destroy(gameObject.transform.FindChild("ShapeMeteorite").gameObject);
		gameObject.GetComponent<Detonator>().Explode();
	}

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {
		UserState userState = GameController.cars[collision.gameObject.name];
		Vector3 direction = userState.rigid.velocity.normalized;
		userState.rigid.AddForce(new Vector3(direction.x, 0, Mathf.Abs(direction.z)) * attackPower * (-1), ForceMode.Impulse);
		int carStatus = 0;
		if(GameController.instance.oneKillMode) {
			carStatus = 2;
			userState.record = TimerController.instance.pastTime;
		}
		StartCoroutine(AfterCollisionEnter(
			explosionSound.clip.length, 
			userState.obj.name, carStatus, collision));
	}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {
		UserState carState = GameController.cars[collision.gameObject.name];
		GameController.instance.UpdateUserCondition(carState.obj.name, 0);
		if(carState.status == 2 && GameController.instance.oneKillMode) {
			GameController.instance.MissGame(carState.obj.name);
		}
	}

}
