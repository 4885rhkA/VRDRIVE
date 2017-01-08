using UnityEngine;
using System.Collections;

/// Class for the creating meteorite continuously
public class Meteorites : MonoBehaviour {

	[SerializeField] private GameObject meteorite;
	[SerializeField] private float createInterval = 4;

	private float[] createMeteoriteZPositions = new float[5]{-18f, -9f, 0f, 9f, 18f};

	/// <summary>Start to create the meteorite on interval.</summary>
	public void StartRockFalling() {
		if(meteorite != null) {
			StartCoroutine(CreateMeteorite());
		}
		else {
			Debug.LogWarning("The system cannnot find the meteorite's prefab.");
		}
	}

	/// <summary>Create the meteorite on interval.</summary>
	private IEnumerator CreateMeteorite() {
		while (true) {
			yield return new WaitForSeconds(createInterval);
			GameObject newMeteorite = Instantiate(
				meteorite, new Vector3(createMeteoriteZPositions[Random.Range(0, 5)], gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation
			) as GameObject;
			newMeteorite.transform.parent = gameObject.transform;
		}
	}

}
