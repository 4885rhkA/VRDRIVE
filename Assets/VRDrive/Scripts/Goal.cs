using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Goal : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	private Color fontColor = new Color();
	private string colorGoal = "#BC151CFF";
	private string colorRecord = "#FFFFFFFF";

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carMessage = userState.message;
		Text carMessageText = carMessage.transform.FindChild("MessageText").GetComponent<Text>();
		userState.record = TimerController.instance.pastTime;
		if(ColorUtility.TryParseHtmlString(colorGoal, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carMessageText, "GOAL!!", fontColor);
		}
		else {
			Debug.LogWarning("The color" + colorGoal + "cannnot convert into Color class.");
		}
		ViewerController.instance.ChangeRawImageState(carMessage.GetComponent<RawImage>(), true);
		ViewerController.instance.ChangeTextState(carMessageText, true);
		SoundController.instance.ShotClipSound("goal");
		StartCoroutine(AfterTriggerEnter(SoundController.instance.GetClipLength("goal"), userState.obj.name, 1, collider));
	}

	/// <summary>After collider occurs, do  action.</summary>
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
		StartCoroutine(AddCharacterContinuouslyForResult(carResultText, resultTimeText.ToCharArray()));
	}

	/// <summary>When collision occurs, do User's action.</summary>
	/// <param name="collision">User's collision</param>
	protected override void CollisionActionForUser(Collision collision) {}

	/// <summary>After collision occurs, do action.</summary>
	protected override void AfterCollisionEnterAction(Collision collision) {}

	/// <summary>Change the Vignette in view.</summary>
	/// <param name="camera">Camera GameObject</param>
	public IEnumerator AddCharacterContinuouslyForResult(Text carResultText, char[] resultTimeTextArray) {
		float clipLength = SoundController.instance.GetClipLength("record");
		foreach(char resultTimeText in resultTimeTextArray) {
			yield return new WaitForSeconds(clipLength);
			string newResultTimeText = carResultText.text + resultTimeText;
			ViewerController.instance.ChangeTextContent(carResultText, newResultTimeText, fontColor);
			SoundController.instance.ShotClipSound("record");
		}
	}

}
