using UnityEngine;
using System.Collections;

/// <summary>
/// Stage controller.
/// </summary>
public class StageController : MonoBehaviour {

	public static StageController instance;

	void Awake() {
		instance = this;
	}

	[SerializeField] private bool meteorites = true;
	[SerializeField] private bool sppedUpBoards = true;
	[SerializeField] private bool checkList = true;
	[SerializeField] private bool tunnnels = true;

	/// <summary>
	/// Sets the condition.
	/// </summary>
	public void SetCondition() {
		if(GameObject.Find("SpeedUpBoards") != null && !sppedUpBoards) {
			GameObject.Find("SpeedUpBoards").gameObject.SetActive(false);
		}
		if(GameObject.Find("CheckList") != null && !checkList) {
			GameObject.Find("CheckList").gameObject.SetActive(false);
		}
		if(GameObject.Find("Tunnels") != null && !tunnnels) {
			GameObject.Find("Tunnels").gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Starts the gimmick.
	/// </summary>
	public void StartGimmick() {
		if(GameObject.Find("Meteorites") != null && meteorites) {
			GameObject.Find("Meteorites").GetComponent<Meteorites>().StartRockFalling();
		}
	}

}
