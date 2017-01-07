using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

	public static StageController instance;

	void Awake() {
		instance = this;
	}

	[SerializeField] private bool meteorites = true;
	[SerializeField] private bool sppedUpBoards = true;
	[SerializeField] private bool checks = true;
	[SerializeField] private bool tunnnels = true;


	public void SetCondition() {
		if(GameObject.Find ("SpeedUpBoards") != null && !sppedUpBoards) {
			GameObject.Find ("SpeedUpBoards").gameObject.SetActive(false);
		}
		if(GameObject.Find ("Checks") != null && !checks) {
			GameObject.Find ("Checks").gameObject.SetActive(false);
		}
		if(GameObject.Find ("Tunnels") != null && !tunnnels) {
			GameObject.Find ("Tunnels").gameObject.SetActive(false);
		}
	}

	/// <summary>Start the gimmick after finishing the count sound.</summary>
	public void StartGimmick() {
		if (GameObject.Find ("Meteorites") != null && meteorites) {
			GameObject.Find ("Meteorites").GetComponent<Meteorites> ().StartRockFalling ();
		}
	}

}
