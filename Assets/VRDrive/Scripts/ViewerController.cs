using UnityEngine;
using System;
using UnityEngine.UI;

public class ViewerController : MonoBehaviour {

	/// <summary>Set the timer text in view.</summary>
	/// <param name="timerText">The target Text Component</param>
	/// <param name="pastTime">The time between start time and now</param>
	public void SetTimerTextToView(Text timerText, TimeSpan pastTime) {
		timerText.text = pastTime.Minutes.ToString().PadLeft(1, '0') + ':' + pastTime.Seconds.ToString().PadLeft(2, '0') + ':' + pastTime.Milliseconds.ToString().PadLeft(3, '0');
	}

	/// <summary>Decide whether showing the text or not in view.</summary>
	/// <param name="text">The target Text Component</param>
	/// <param name="state">The trigger for showing text or not</param>
	public void ChangeTextState(Text text, bool state) {
		text.enabled = state;
	}

	/// <summary>Change the text content and color in view.</summary>
	/// <param name="text">The target Text Component</param>
	/// <param name="content">New text of Text Component</param>
	/// <param name="color">The color for the text</param>
	public void ChangeTextContent(Text text, string content, Color color) {
		text.text = content;
		text.color = color;
	}

}
