using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimerController : MonoBehaviour {

	private DateTime startTime = DateTime.Now;
	public TimeSpan pastTime = new TimeSpan(0, 0, 0);

	private AudioSource source;
	[SerializeField] private AudioClip countClip;
	[SerializeField] private AudioClip goClip;
	[SerializeField] private AudioClip StageClip;

	public TimeSpan getPastTime() {
		return pastTime;
	}

	void Start() {
		source = gameObject.GetComponent<AudioSource>();
		source.clip = countClip;
		source.Play();
		StartCoroutine(StartGame(countClip.length));
	}

	void Update() {
		TimerCount();
	}

	/// <summary>start the time</summary>
	private IEnumerator StartGame(float clipLength) {
		yield return new WaitForSeconds(clipLength);
		source.clip = StageClip;
		source.PlayOneShot(goClip);
		source.loop = true;
		source.Play();
		startTime = DateTime.Now;
		gameObject.GetComponent<GameController>().ChangeAllStatus(0);
	}

	/// <summary>count the time</summary>
	private void TimerCount() {
		pastTime = DateTime.Now - startTime;
	}

}
