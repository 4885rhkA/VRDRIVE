using UnityEngine;
using System.Collections;

/// Control class for the each user's camera
public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] public float cameraDistanceY;
	[SerializeField] public float cameraDistanceZ;
	[SerializeField] public float cameraRotationUpDown;

	void Awake() {
		instance = this;
	}

}
