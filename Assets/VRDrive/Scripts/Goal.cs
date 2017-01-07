using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// Class for defined action when collision between user's car and goal
public class Goal : Incident {

	private Color fontColor = new Color();
	private string colorGoal = "#BC151CFF";
	private string colorRecord = "#FFFFFFFF";
	private string colorResult = "#FFFFFFFF";

	void Awake() {
		collisionFlag = new bool[6, 2] {
			{false, true}, 		// OnTriggerEnter
			{false, false}, 	// OnCollisionEnter
			{false, false}, 	// OnTriggerStay
			{false, false},		// OnCollisionStay
			{false, false}, 	// OnTriggerExit
			{false, false}		// OnCollisionExit
		};
	}

	/// <summary>When collider/collision occurs, do object's action.</summary>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>When collider/collision occurs, do user's action.</summary>
	/// <param name="userName">The name for user</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet (userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if (userState.Status < 1) {
			GameObject message = userObject.Message;
			Text messageText = message.transform.FindChild("MessageText").GetComponent<Text>();
			GameController.instance.UpdateRecord (userObject.Obj.name, TimerController.instance.PastTime);
			if(ColorUtility.TryParseHtmlString(colorGoal, out fontColor)) {
				ViewerController.instance.ChangeTextContent(messageText, "GOAL!", fontColor);
			}
			ViewerController.instance.ChangeRawImageState(message.GetComponent<RawImage>(), true);
			ViewerController.instance.ChangeTextState(0, messageText, true);
			SoundController.instance.ShotClipSound("goal");
			GameController.instance.UpdateUserStatus(userObject.Obj.name, 1);
			StartCoroutine(AfterCollisionAction(SoundController.instance.GetClipLength("goal"), userSet));
		}
	}

	/// <summary>After collider/collision occurs, do action.</summary>
	/// <param name="userSet">User's State and Object</param>
	private IEnumerator AfterCollisionAction(float delay, UserSet userSet) {
		yield return new WaitForSeconds(delay);

		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		GameObject result = userObject.Result;
		Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();
		string resultTimeText = ViewerController.instance.GetTimerText(userState.Record);
		if(ColorUtility.TryParseHtmlString(colorRecord, out fontColor)) {
			ViewerController.instance.ChangeTextContent(resultText, "TIME ", fontColor);
		}
		ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(0, resultText, true);
		SoundController.instance.ShotClipSound("record");
		StartCoroutine(AddCharacterContinuouslyForResult(resultText, resultTimeText.ToCharArray()));
	}

	private IEnumerator AddCharacterContinuouslyForResult(Text resultText, char[] resultTimeTextArray) {
		float clipLength = SoundController.instance.GetClipLength("record");

		foreach(char resultTimeText in resultTimeTextArray) {
			yield return new WaitForSeconds(clipLength);
			string newResultTimeText = resultText.text + resultTimeText;

			if (ColorUtility.TryParseHtmlString (colorResult, out fontColor)) {
				ViewerController.instance.ChangeTextContent(resultText, newResultTimeText, fontColor);
			}
			SoundController.instance.ShotClipSound("record");
		}
	}

}
