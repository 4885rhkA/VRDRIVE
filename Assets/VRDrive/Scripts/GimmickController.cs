using UnityEngine;
using System.Collections;

public class GimmickController : MonoBehaviour {

	public static GimmickController instance;

	public Vector3 localGravity;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	/// <summary>When collider occurs, decide each Object's action by GameController's reply.</summary>
	/// <param name="fromObj">Attack Object</param>
	/// <param name="toObj">Attacked Object</param>
	public void ColliderAction(GameObject fromObj, GameObject toObj) {
	}

	/// <summary>When collision occurs, decide each Object's action by GameController's reply.</summary>
	/// <param name="fromObj">Attack Object</param>
	/// <param name="toObj">Attacked Object</param>
	public void CollisionAction(GameObject fromObj, Collision toObj) {
	}

}
