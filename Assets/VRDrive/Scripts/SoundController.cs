using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	private AudioSource source;
	[SerializeField] private AudioClip countClip;
	[SerializeField] private AudioClip goClip;
	[SerializeField] private AudioClip stageClip;
	[SerializeField] private AudioClip goalClip;

	void Start() {
		source = gameObject.GetComponent<AudioSource>();
		source.clip = countClip;
		source.Play();
	}

	/// <summary>Return the length of Clip for standby.</summary>
	/// <param name="targetClip">The Clip which you want to get the length</param>
	public float getClipLength(string targetClip) {
		switch(targetClip) {
			case "count":
				return countClip.length;
			case "go":
				return goClip.length;
			case "stage":
				return stageClip.length;
			case "goal":
				return goalClip.length;
			default:
				Debug.LogWarning(targetClip + "Clip cannnot be found. So return 0.");
				return 0;
		}
	}

	/// <summary>Start the stageClip with goClip.</summary>
	public void StartStageSound() {
		source.clip = stageClip;
		source.PlayOneShot(goClip);
		source.loop = true;
		source.Play();
	}

	/// <summary>Call goalClip.</summary>
	public void GoalStageSound() {
		source.PlayOneShot(goalClip);
	}

}
