using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	private AudioSource source;
	[SerializeField] private AudioClip countClip;
	[SerializeField] private AudioClip goClip;
	[SerializeField] private AudioClip StageClip;

	void Start() {
		source = gameObject.GetComponent<AudioSource>();
		source.clip = countClip;
		source.Play();
	}

	/// <summary>Return the length of countClip for standby.</summary>
	public float getCountClipLength() {
		return countClip.length;
	}

	/// <summary>Start the stageClip with goClip.</summary>
	public void StartStageSound() {
		source.clip = StageClip;
		source.PlayOneShot(goClip);
		source.loop = true;
		source.Play();
	}

}
