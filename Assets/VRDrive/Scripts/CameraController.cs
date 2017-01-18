using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Camera controller.
/// </summary>
public class CameraController : MonoBehaviour {

	public static CameraController instance;

	[SerializeField] private float interval = 0.1f;
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

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
	}

	/// <summary>
	/// Creates the key for screenshot.
	/// </summary>
	/// <returns>The key for screenshot.</returns>
	/// <param name="playerName">Player name.</param>
	/// <param name="incidentName">Incident name.</param>
	public string CreateKeyForScreenshot(string playerName, string incidentName) {
		return playerName + "-" + incidentName;
	}

	/// <summary>
	/// Captures the and save screenshots.
	/// </summary>
	/// <returns>The and save screenshots.</returns>
	/// <param name="key">Key.</param>
	public IEnumerator CaptureAndSaveScreenshots(string key) {
		if(!playerScreenshotList.ContainsKey(key)) {
			playerScreenshotList.Add(key, new List<Texture2D>());
		}

		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
		yield return new WaitForEndOfFrame();
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
		texture.Apply();
		playerScreenshotList[key].Add(texture);
		yield return new WaitForSeconds(interval);
	}

}
