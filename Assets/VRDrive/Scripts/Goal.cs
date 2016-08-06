using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Class for defined action when collision between user's car and goal
public class Goal : Incident {

	private Color fontColor = new Color();
	private string colorGoal = "#BC151CFF";
	private string colorRecord = "#FFFFFFFF";

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollidedActionForMyself() {}

	/// <summary>When collider occurs, do user's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carMessage = userState.message;
		Text carMessageText = carMessage.transform.FindChild("MessageText").GetComponent<Text>();
		userState.record = TimerController.instance.pastTime;
		if(ColorUtility.TryParseHtmlString(colorGoal, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carMessageText, "GOAL!", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorGoal + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(carMessage.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(carMessageText, true);
		SoundController.instance.ShotClipSound("goal");
		StartCoroutine(AfterTriggerEnter(SoundController.instance.GetClipLength("goal"), userState.obj.name, 1, collider));
	}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carResult = userState.result;
		Text carResultText = carResult.transform.FindChild("ResultText").GetComponent<Text>();
		string resultTimeText = ViewerController.instance.GetTimerText(userState.record);
		if(ColorUtility.TryParseHtmlString(colorRecord, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carResultText, "TIME ", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorRecord + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(carResult.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(carResultText, true);
		SoundController.instance.ShotClipSound("record");
		StartCoroutine(GameController.instance.AddCharacterContinuouslyForResult(carResultText, resultTimeText.ToCharArray()));
	}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
