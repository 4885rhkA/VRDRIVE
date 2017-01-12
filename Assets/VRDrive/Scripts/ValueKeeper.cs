using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Value keeper.
/// </summary>
public class ValueKeeper : MonoBehaviour {

	private Dictionary<string, UserState> userStateList = new Dictionary<string, UserState>();
	private Dictionary<string, List<Texture2D>> playerScreenshotList = new Dictionary<string, List<Texture2D>>();
	private string sceneAfterEvaluation;

	public Dictionary<string, UserState> UserStateList {
		get {
			return userStateList;
		}
		set {
			userStateList = value;
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

	public string SceneAfterEvaluation {
		get {
			return sceneAfterEvaluation;
		}
		set {
			sceneAfterEvaluation = value;
		}
	}
}
