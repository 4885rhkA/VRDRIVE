using UnityEngine;
using System;

/// <summary>
/// Timer controller.
/// </summary>
public class TimerController : MonoBehaviour {

	public static TimerController instance;

	private DateTime startTime = DateTime.Now;
	public TimeSpan pastTime = new TimeSpan(0, 0, 0);

	public TimeSpan PastTime {
		get {
			return pastTime;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		instance = this;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		TimerCount();
	}

	/// <summary>
	/// Resets the start time.
	/// </summary>
	public void ResetStartTime() {
		startTime = DateTime.Now;
	}

	/// <summary>
	/// Timers the count.
	/// </summary>
	private void TimerCount() {
		pastTime = DateTime.Now - startTime;
	}

}
