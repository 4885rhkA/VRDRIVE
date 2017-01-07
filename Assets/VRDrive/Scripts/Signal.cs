using UnityEngine;
using System.Collections;

/// Class for turning signal on interval
public class Signal : MonoBehaviour {

	[SerializeField] private int greenColorTime = 10;
	[SerializeField] private int yellowColorTime = 1;
	[SerializeField] private int redColorTime = 10;

	private int status = 0;

	private MeshRenderer greenRenderer;
	private MeshRenderer yellowRenderer;
	private MeshRenderer redRenderer;

	void Start () {
		greenRenderer = gameObject.transform.FindChild ("Green").gameObject.GetComponent<MeshRenderer>();
		yellowRenderer = gameObject.transform.FindChild ("Yellow").gameObject.GetComponent<MeshRenderer>();
		redRenderer = gameObject.transform.FindChild ("Red").gameObject.GetComponent<MeshRenderer>();
		StartCoroutine(TurnSignal());
	}

	/// <summary>Turn the signal on interval.</summary>
	private IEnumerator TurnSignal() {
		while (true) {
			int waitTime = 10;
			status = (status + 1) % 3;
			switch (status) {
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

	/// <summary>Get the status of signal</summary>
	/// <returns>
	///  	2:Red
	///  	1:Yellow
	/// 	0:Green
	/// </returns>
	public int GetStatus() {
		return status;
	}

}
