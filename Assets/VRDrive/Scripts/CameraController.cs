using UnityEngine;
using System.Collections;

/// Control class for the each user's camera
public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] public float cameraDistanceY;
	[SerializeField] public float cameraDistanceZ;
	[SerializeField] public float cameraRotationUpDown;

	[SerializeField] private string directoryForScreenshot = "VRDrive/Result/Screenshots";
	[SerializeField] private float intervalForCapture = 0.5f;
	[SerializeField] private float delayForCaptureAfterTriggerExit = 2f;

	public string DirectoryForScreenshot {
		get {
			return directoryForScreenshot;
		}
	}

	public float IntervalForCapture {
		get {
			return intervalForCapture;
		}
	}

	public float DelayForCaptureAfterTriggerExit {
		get {
			return delayForCaptureAfterTriggerExit;
		}
	}

	void Awake() {
		instance = this;
	}

}
