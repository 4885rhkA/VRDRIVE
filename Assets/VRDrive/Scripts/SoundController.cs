using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Control class for the each user's sound
public class SoundController : MonoBehaviour {

	public static SoundController instance;

	[SerializeField, TooltipAttribute("You do not have to consider order.")] private AudioClip[] clips;

	private AudioSource source;

	/// <value>
	/// You need to add in this, the system sounds, the system clips and the incident clips.
	/// The incident sounds, for example, fireloop sound of Meteorite, must set in the incident's inspector.
	/// Because it must be 3D sounds.
	/// </value>
	private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
		foreach(AudioClip audioClip in clips) {
			audioClips.Add(audioClip.name, audioClip);
		}
	}

	void Start() {
		source = gameObject.GetComponent<AudioSource>();
		source.clip = audioClips["count"];
		source.Play();
	}

	/// <summary>Return the length of <c>AudioClip</c> for standby.</summary>
	/// <param name="clipName">The <c>AudioClip</c> name which you want to get the length</param>
	public float GetClipLength(string clipName) {
		if(audioClips.ContainsKey(clipName)) {
			return audioClips[clipName].length;
		}
		else {
			Debug.LogWarning(clipName + " Clip cannnot be found. So return 0.");
			return 0;
		}
	}

	/// <summary>Shot the sound only one time.</summary>
	/// <param name="clipName">The <c>AudioClip</c> name which you want to shot</param>
	public void ShotClipSound(string clipName) {
		if(audioClips.ContainsKey(clipName)) {
			source.PlayOneShot(audioClips[clipName]);
		}
		else {
			Debug.LogWarning(clipName + " Clip cannnot be found.");
		}
	}

	/// <summary>Start the sound for starting game.</summary>
	public void StartStageSound() {
		source.clip = audioClips["stage"];
		ShotClipSound("go");
		source.loop = true;
		source.Play();
	}

}
