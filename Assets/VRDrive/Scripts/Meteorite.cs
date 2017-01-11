using UnityEngine;
using System.Collections;

/// <summary>
/// Meteorite.
/// </summary>
public class Meteorite : Incident {

	[SerializeField] private int attackPower = 200000;
	[SerializeField] private int[] moveTime = new int[2]{ 10, 11 };
	private float[] moveToPosition;

	private AudioSource[] audioSources;
	private AudioSource loopSound;
	private AudioSource explosionSound;

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ true, false }, 	// OnTriggerEnter
			{ true, true }, 	// OnCollisionEnter
			{ false, false }, 	// OnTriggerStay
			{ false, false },	// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }	// OnCollisionExit
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		audioSources = gameObject.GetComponents<AudioSource>();
		loopSound = audioSources[0];
		explosionSound = audioSources[1];
		if(moveTime[0] < moveTime[1]) {
			moveToPosition = new float[3]{ gameObject.transform.position.x, gameObject.transform.position.y, -20 };
			iTween.RotateAdd(gameObject.transform.FindChild("ShapeMeteorite").gameObject, iTween.Hash("x", Random.Range(0, 2) * 360, "y", Random.Range(0, 2) * 360, "z", Random.Range(0, 2) * 360, "time", Random.Range(10, 21), "easeType", "linear", "loopType", "loop"));
			iTween.MoveTo(gameObject, iTween.Hash("x", moveToPosition[0], "y", moveToPosition[1], "z", moveToPosition[2], "time", Random.Range(moveTime[0], moveTime[1]), "easeType", "linear"));
		}
		else {
			Debug.LogWarning("You need to set the correct number, the range of the moving time for the meteorite.");
		}
	}

	/// <summary>
	/// Collisions the action for myself.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
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

	/// <summary>
	/// Collisions the action for user.
	/// </summary>
	/// <param name="userName">User name.</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if(userState.Condition != 1) {
			Vector3 direction = userObject.Obj.GetComponent<Rigidbody>().velocity.normalized;
			userObject.Obj.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, 0, Mathf.Abs(direction.z)) * attackPower * (-1), ForceMode.Impulse);

			if(GameController.instance.OneKillMode) {
				GameController.instance.MissGame(userObject.Obj.name);
			}
			else {
				GameController.instance.UpdateUserCondition(userObject.Obj.name, 1);
				GameController.instance.UpdateUserStatus(userObject.Obj.name, 0);
				StartCoroutine(AfterCollisionAction(explosionSound.clip.length, userObject.Obj.name));
			}
		}
	}

	/// <summary>
	/// Afters the collision action.
	/// </summary>
	/// <returns>The collision action.</returns>
	/// <param name="delay">Delay.</param>
	/// <param name="userName">User name.</param>
	private IEnumerator AfterCollisionAction(float delay, string userName) {
		yield return new WaitForSeconds(delay);

		GameController.instance.UpdateUserCondition(userName, 0);
	}

}
