using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnderGround : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	private Color fontColor = new Color();
	private string colorMiss = "#FFFFFFFF";

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		userState.record = TimerController.instance.pastTime;
		StartCoroutine(AfterTriggerEnter(0, userState.obj.name, 2, collider));
	}

	/// <summary>After collider occurs, do  action.</summary>
	protected override void AfterTriggerEnterAction(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carResult = userState.result;
		Text carResultText = carResult.transform.FindChild("ResultText").GetComponent<Text>();
		if(ColorUtility.TryParseHtmlString(colorMiss, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carResultText, "MISS......", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorMiss + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(carResult.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(carResultText, true);
		SoundController.instance.ShotClipSound("miss");

	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
