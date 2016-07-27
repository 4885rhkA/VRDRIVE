using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Goal : Incident {

	/// <summary>When collider/collision occurs, do Object's action.</summary>
	protected override void CollidedActionForMyself() {}

	private Color fontColor = new Color();
	private string colorGoal = "#BC151CFF";
	private string colorResult = "#FFFFFFFF";

	/// <summary>When collider occurs, do User's action.</summary>
	/// <param name="collider">User's collider</param>
	protected override void ColliderActionForUser(Collider collider) {
		UserState userState = GameController.cars[collider.gameObject.name];
		GameObject carMessage = userState.message;
		Text carMessageText = carMessage.transform.FindChild("MessageText").GetComponent<Text>();
		userState.record = TimerController.instance.pastTime;
		ViewerController.instance.ChangeRawImageState(carMessage.GetComponent<RawImage>(), true);
		if(ColorUtility.TryParseHtmlString(colorGoal, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carMessageText, "GOAL!!", fontColor);
		}
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
		ViewerController.instance.ChangeRawImageState(carResult.GetComponent<RawImage>(), true);
		if(ColorUtility.TryParseHtmlString(colorResult, out fontColor)) {
			ViewerController.instance.ChangeTextContent(carResultText, "TIME ", fontColor);
			SoundController.instance.ShotClipSound("record");
			StartCoroutine(AddCharacterContinuouslyForResult(carResultText, resultTimeText.ToCharArray()));

		}
		ViewerController.instance.ChangeTextState(carResultText, true);

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
