using UnityEngine;
using System.Collections;

/// Control class for the each user's camera
public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] public float cameraDistanceY;
	[SerializeField] public float cameraDistanceZ;
	[SerializeField] public float cameraRotationUpDown;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

    /// <summary>Set each user's camera parameter for 2D.</summary>
    /// <param name="carCameraTransform">The <c>Transform</c> of the user's camera/param>
    /// <param name="carTransform">The <c>Transform</c> of the user's car</param>
    public void SetCameraPositionAndRotation2D(Transform carCameraTransform, Transform carTransform) {
        Vector3 targetPosition = new Vector3(carTransform.position.x, cameraRotationUpDown, carTransform.position.z);
        carCameraTransform.position = targetPosition + Vector3.up * cameraDistanceY + carTransform.forward * cameraDistanceZ;
        carCameraTransform.LookAt(targetPosition);
    }

    /// <summary>Set each user's camera parameter for 3D.</summary>
    /// <param name="carCameraTransform">The <c>Transform</c> of the user's camera/param>
    /// <param name="carTransform">The <c>Transform</c> of the user's car</param>
    public void SetCameraPositionAndRotation3D(Transform carCameraTransform, Transform carTransform) {
		carCameraTransform.position = carTransform.position + carTransform.forward * cameraDistanceZ + Vector3.up * cameraDistanceY;
		carCameraTransform.LookAt(carTransform.position + Vector3.up * cameraRotationUpDown);
	}

}
