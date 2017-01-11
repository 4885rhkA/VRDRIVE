using UnityEngine;
using System;

/// Control class for the each user's timer
public class TimerController : MonoBehaviour {

	public static TimerController instance;

	private DateTime startTime = DateTime.Now;
	public TimeSpan pastTime = new TimeSpan(0, 0, 0);

	public TimeSpan PastTime {
		get {
			return pastTime;
		}
	}

	void Awake() {
		instance = this;
	}

	void Update() {
		TimerCount();
	}

	/// <summary>Reset the starttime. It equals to start timer when starting the game.</summary>
	public void ResetStartTime() {
		startTime = DateTime.Now;
	}

	/// <summary>Count the time.</summary>
	private void TimerCount() {
		pastTime = DateTime.Now - startTime;
	}

}
