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
		UserSet userSet = GameController.instance.GetUserSet (collider.gameObject.name);
		UserObject userObject = userSet.UserObject;

		GameObject message = userObject.Message;
		Text messageText = message.transform.FindChild("MessageText").GetComponent<Text>();
		GameController.instance.UpdateRecord (userObject.Obj.name, TimerController.instance.PastTime);
		if(ColorUtility.TryParseHtmlString(colorGoal, out fontColor)) {
			ViewerController.instance.ChangeTextContent(messageText, "GOAL!", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorGoal + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(message.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(messageText, true);
		SoundController.instance.ShotClipSound("goal");
		StartCoroutine(AfterTriggerEnter(SoundController.instance.GetClipLength("goal"), userObject.Obj.name, 1, collider));
	}

	/// <summary>After collider occurs, do action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void AfterTriggerEnterAction(Collider collider) {
		UserSet userSet = GameController.instance.GetUserSet (collider.gameObject.name);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		GameObject result = userObject.Result;
		Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();
		string resultTimeText = ViewerController.instance.GetTimerText(userState.Record);
		if(ColorUtility.TryParseHtmlString(colorRecord, out fontColor)) {
			ViewerController.instance.ChangeTextContent(resultText, "TIME ", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorRecord + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(resultText, true);
		SoundController.instance.ShotClipSound("record");
		StartCoroutine(GameController.instance.AddCharacterContinuouslyForResult(resultText, resultTimeText.ToCharArray()));
	}

	/// <summary>When collision occurs, do user's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void AfterCollisionEnterAction(Collision collision) {}

}
