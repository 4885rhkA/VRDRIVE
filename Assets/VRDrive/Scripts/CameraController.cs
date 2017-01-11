using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Control class for the each user's camera
public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] private float interval = 0f;
	private Dictionary<string, List<Texture2D>> playerScreenshotList = new Dictionary<string, List<Texture2D>>();

	public float Intereval {
		get {
			return interval;
		}
	}

	public Dictionary<string, List<Texture2D>> PlayerScreenshotList {
		get {
			return playerScreenshotList;
		}
		set {
			playerScreenshotList = value;
		}
	}

	void Awake() {
		instance = this;
	}

	public string CreateKeyForScreenshot(string playerName, string incidentName) {
		return playerName + "-" + incidentName;
	}

	public IEnumerator CaptureAndSaveScreenshots(string key) {
		if (!playerScreenshotList.ContainsKey (key)) {
			playerScreenshotList.Add (key, new List<Texture2D> ());
		}

		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
		yield return new WaitForEndOfFrame();
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
		texture.Apply();
		playerScreenshotList [key].Add (texture);
	}

}
