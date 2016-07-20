using UnityEngine;
using System;
using UnityEngine.UI;

public class ViewerController : MonoBehaviour {

	/// <summary>set the timer text to view</summary>
	public void SetTimerTextToView(Text timerText, TimeSpan pastTime) {
		timerText.text = pastTime.Minutes.ToString().PadLeft(1, '0') + ':' + pastTime.Seconds.ToString().PadLeft(2, '0') + ':' + pastTime.Milliseconds.ToString().PadLeft(3, '0');
	}

	/// <summary>whether show the time or not</summary>
	public void ChangeTimerState(Text timerText, bool state) {
		timerText.enabled = state;
	}

}
