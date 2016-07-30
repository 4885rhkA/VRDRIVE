using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] private float cameraDistanceY;
	[SerializeField] private float cameraDistanceZ;
	[SerializeField] private float cameraRotationUpDown;


	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void ChaseCar (Transform carCameraTransform, Transform carTransform) {
		carCameraTransform.position = carTransform.position + carTransform.forward * cameraDistanceZ + Vector3.up * cameraDistanceY;
		carCameraTransform.LookAt(carTransform.position + Vector3.up * cameraRotationUpDown);
	}

}
