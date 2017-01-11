using UnityEngine;
using System.Collections;

/// <summary>
/// Signal.
/// </summary>
public class Signal : MonoBehaviour {

	[SerializeField] private bool startRedColorFlag = false;
	[SerializeField] private int greenColorTime = 10;
	[SerializeField] private int yellowColorTime = 1;
	[SerializeField] private int redColorTime = 11;

	private int DEFAULTSTOPTIME = 10;

	private int status = 2;

	private MeshRenderer greenRenderer;
	private MeshRenderer yellowRenderer;
	private MeshRenderer redRenderer;

	/// <returns>
	///  	2:Red
	///  	1:Yellow
	/// 	0:Green
	/// </returns>
	public int Status {
		get {
			return status;
		}
		set {
			status = value;
		}
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		greenRenderer = gameObject.transform.FindChild("Green").gameObject.GetComponent<MeshRenderer>();
		yellowRenderer = gameObject.transform.FindChild("Yellow").gameObject.GetComponent<MeshRenderer>();
		redRenderer = gameObject.transform.FindChild("Red").gameObject.GetComponent<MeshRenderer>();

		// Color select when game started
		if(startRedColorFlag) {
			status = 1;
		}

		StartCoroutine(TurnSignal());
	}

	/// <summary>
	/// Turns the signal.
	/// </summary>
	/// <returns>The signal.</returns>
	private IEnumerator TurnSignal() {
		while(true) {
			int waitTime = DEFAULTSTOPTIME;
			Status = (Status + 1) % 3;
			switch(Status) {
				case 0:
					waitTime = greenColorTime;
					greenRenderer.enabled = true;
					yellowRenderer.enabled = false;
					redRenderer.enabled = false;
					break;
				case 1:
					waitTime = yellowColorTime;
					greenRenderer.enabled = false;
					yellowRenderer.enabled = true;
					redRenderer.enabled = false;
					break;
				case 2:
					waitTime = redColorTime;
					greenRenderer.enabled = false;
					yellowRenderer.enabled = false;
					redRenderer.enabled = true;
					break;
			}
			yield return new WaitForSeconds(waitTime);
		}
	}

}
