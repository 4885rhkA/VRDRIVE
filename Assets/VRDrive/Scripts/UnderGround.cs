using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnderGround : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	private Color fontColor = new Color();
	private string colorRetire = "#BC151CFF";

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carMessage = userState.message;
		Text carMessageText = carMessage.transform.FindChild("MessageText").GetComponent<Text>();
		userState.record = TimerController.instance.pastTime;
		ViewerController.instance.ChangeRawImageState(carMessage.GetComponent<RawImage>(), true);
		if(ColorUtility.TryParseHtmlString(colorRetire, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carMessageText, "MISS!!", fontColor);
		}
		ViewerController.instance.ChangeTextState(carMessageText, true);
		SoundController.instance.ShotClipSound("goal");
		StartCoroutine(AfterTriggerEnter(0, userState.obj.name, 2, collider));
	}

	/// <summary>After collider occurs, do  action.</summary>
	protected override void AfterTriggerEnterAction(Collider collider) {}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
