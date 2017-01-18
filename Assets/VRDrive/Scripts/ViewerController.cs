using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Viewer controller.
/// </summary>
public class ViewerController : MonoBehaviour {

	public static ViewerController instance;

	private float vignettingTime = 2;
	private float baseVignetteIntensity = 0.4f;

	void Awake() {
		instance = this;
	}

	/// <summary>
	/// Gets the timer text.
	/// </summary>
	/// <returns>The timer text.</returns>
	/// <param name="pastTime">Past time.</param>
	public string GetTimerText(TimeSpan pastTime) {
		return pastTime.Minutes.ToString().PadLeft(1, '0') + ':' + pastTime.Seconds.ToString().PadLeft(2, '0') + ':' + pastTime.Milliseconds.ToString().PadLeft(3, '0');
	}


	/// <summary>
	/// Changes the state of the text.
	/// </summary>
	/// <returns>The text state.</returns>
	/// <param name="text">Text.</param>
	/// <param name="state">If set to <c>true</c> state.</param>
	/// <param name="delay">Delay.</param>
	public IEnumerator ChangeTextState(Text text, bool state, float delay = 0) {
		yield return new WaitForSeconds(delay);
		text.enabled = state;
	}

	/// <summary>
	/// Changes the state of the raw image.
	/// </summary>
	/// <param name="image">Image.</param>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void ChangeRawImageState(RawImage image, bool state) {
		image.enabled = state;
	}

	/// <summary>
	/// Changes the content of the image.
	/// </summary>
	/// <param name="image">Image.</param>
	/// <param name="content">Content.</param>
	public void ChangeImageContent(RawImage image, string content) {
		if(content != null) {
			Debug.Log("ss");
			image.texture = Resources.Load(content) as Texture2D;
		}
	}

	/// <summary>
	/// Changes the content of the text.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="content">Content.</param>
	/// <param name="color">Color.</param>
	public void ChangeTextContent(Text text, string content, Color color) {
		if(content != null) {
			text.text = content;
		}
		text.color = color;
	}

	/// <summary>
	/// Changes the content of the text mesh.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="content">Content.</param>
	/// <param name="color">Color.</param>
	public void ChangeTextMeshContent(TextMesh text, string content, Color color) {
		if(content != null) {
			text.text = content;
		}
		text.color = color;
	}

	/// <summary>
	/// Changes the motion blur.
	/// </summary>
	/// <param name="camera">Camera.</param>
	/// <param name="blurAmount">Blur amount.</param>
	public void ChangeMotionBlur(GameObject camera, float blurAmount) {
		camera.GetComponent<MotionBlur>().blurAmount = Mathf.Clamp(blurAmount, 0, 1);
	}

	/// <summary>
	/// 
	/// Changes the damage view.
	/// </summary>
	/// <returns>The damage view.</returns>
	/// <param name="camera">Camera.</param>
	public IEnumerator ChangeDamageView(GameObject camera) {
		VignetteAndChromaticAberration vignette = camera.GetComponent<VignetteAndChromaticAberration>();
		float startTime = Time.time;
		while(vignettingTime - (Time.time - startTime) > 0) {
			float rate = vignettingTime / (vignettingTime + Time.time - startTime);
			vignette.intensity = baseVignetteIntensity * Mathf.Clamp(rate, 0, 1);
			yield return new WaitForEndOfFrame();
		}
		vignette.intensity = 0;
	}

}
