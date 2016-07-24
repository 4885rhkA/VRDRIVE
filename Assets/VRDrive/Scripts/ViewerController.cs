using UnityEngine;
using System;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class ViewerController : MonoBehaviour {

	public static ViewerController instance;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

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

	/// <summary>Decide whether showing the image or not in view.</summary>
	/// <param name="text">The target RawImage Component</param>
	/// <param name="state">The trigger for showing image or not</param>
	public void ChangeRawImageState(RawImage image, bool state) {
		image.enabled = state;
	}

	/// <summary>Change the text content and color in view.</summary>
	/// <param name="text">The target Text Component</param>
	/// <param name="content">New text of Text Component</param>
	/// <param name="color">The color for the text</param>
	public void ChangeTextContent(Text text, string content, Color color) {
		text.text = content;
		text.color = color;
	}

	/// <summary>Change the motion blour in view.</summary>
	/// <param name="camera">Camera GameObject</param>
	/// <param name="blurAmount">Blur Amount(from 0 to 1)</param>
	public void ChangeMotionBlur(GameObject camera, float blurAmount) {
		camera.GetComponent<MotionBlur>().blurAmount = Mathf.Clamp(blurAmount, 0, 1);
	}

}
