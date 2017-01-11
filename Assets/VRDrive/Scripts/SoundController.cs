using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sound controller.
/// </summary>
public class SoundController : MonoBehaviour {

	public static SoundController instance;

	[SerializeField, TooltipAttribute("You do not have to consider order.")] private AudioClip[] clips;

	private AudioSource source;

	private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
		source = gameObject.GetComponent<AudioSource>();
		audioClips.Clear();
		foreach(AudioClip audioClip in clips) {
			audioClips.Add(audioClip.name, audioClip);
		}
		clips = null;
	}

	/// <summary>
	/// Gets the length of the clip.
	/// </summary>
	/// <returns>The clip length.</returns>
	/// <param name="clipName">Clip name.</param>
	public float GetClipLength(string clipName) {
		if(audioClips.ContainsKey(clipName)) {
			return audioClips[clipName].length;
		}
		else {
			Debug.LogWarning(clipName + " Clip cannnot be found. So return 0.");
			return 0;
		}
	}

	/// <summary>
	/// Shots the clip sound.
	/// </summary>
	/// <param name="clipName">Clip name.</param>
	public void ShotClipSound(string clipName) {
		if(audioClips.ContainsKey(clipName)) {
			source.PlayOneShot(audioClips[clipName]);
		}
		else {
			Debug.LogWarning(clipName + " Clip cannnot be found.");
		}
	}

	/// <summary>
	/// Starts the game sound.
	/// </summary>
	public void StartGameSound() {
		source.clip = audioClips["scene"];
		ShotClipSound("go");
		source.loop = true;
		source.Play();
	}

	/// <summary>
	/// Starts the menu sound.
	/// </summary>
	public void StartMenuSound() {
		source.clip = audioClips["scene"];
		source.loop = true;
		source.Play();
	}

	/// <summary>
	/// Starts the result sound.
	/// </summary>
	public void StartResultSound() {
		source.clip = audioClips["scene"];
		source.loop = true;
		source.Play();
	}

}
