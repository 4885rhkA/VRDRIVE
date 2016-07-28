using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnderGround : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	private Color fontColor = new Color();
	private string colorResult = "#FFFFFFFF";

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
		ViewerController.instance.ChangeRawImageState(carResult.GetComponent<RawImage>(), true);
		if(ColorUtility.TryParseHtmlString(colorResult, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carResultText, "MISS......", fontColor);
			SoundController.instance.ShotClipSound("miss");
		}
		ViewerController.instance.ChangeTextState(carResultText, true);
		UserController.instance.SetFreezingRotation(userState.rigid);
	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
