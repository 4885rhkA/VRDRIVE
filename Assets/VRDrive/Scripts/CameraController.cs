using UnityEngine;
using System.Collections;

/// Control class for the each user's camera
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

	/// <summary>Set each user's camera parameter.</summary>
	/// <param name="carCameraTransform">The <c>Transform</c> of the user's camera/param>
	/// <param name="carTransform">The <c>Transform</c> of the user's car</param>
	public void SetCameraPositionAndRotation (Transform carCameraTransform, Transform carTransform) {
		carCameraTransform.position = carTransform.position + carTransform.forward * cameraDistanceZ + Vector3.up * cameraDistanceY;
		carCameraTransform.LookAt(carTransform.position + Vector3.up * cameraRotationUpDown);
	}

}
