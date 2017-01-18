using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Goal.
/// </summary>
public class Goal : Incident {

	private Color fontColor = new Color();
	private Dictionary<string, string> colorList;
	private Dictionary<string, string> messageList;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		collisionFlag = new bool[6, 2] {
			{ false, true }, 		// OnTriggerEnter
			{ false, false }, 	// OnCollisionEnter
			{ false, false }, 	// OnTriggerStay
			{ false, false },		// OnCollisionStay
			{ false, false }, 	// OnTriggerExit
			{ false, false }		// OnCollisionExit
		};
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		colorList = GameController.instance.ColorList;
		messageList = GameController.instance.MessageList;
	}

	/// <summary>
	/// When collider/collision occurs, do object's action.
	/// </summary>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForMyself(int kindOfCollision) {
	}

	/// <summary>
	/// When collider/collision occurs, do user's action.
	/// </summary>
	/// <param name="userName">The name for user</param>
	/// <param name="kindOfCollision">Kind of collision.</param>
	protected override void CollisionActionForUser(string userName, int kindOfCollision) {
		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;

		GameController.instance.ClearGame(userObject.Obj.name);
		StartCoroutine(AfterCollisionAction(SoundController.instance.GetClipLength("goal"), userObject.Obj.name));
	}

	/// <summary>
	/// Afters the collision action.
	/// </summary>
	/// <returns>The collision action.</returns>
	/// <param name="delay">Delay.</param>
	/// <param name="userName">User name.</param>
	private IEnumerator AfterCollisionAction(float delay, string userName) {
		yield return new WaitForSeconds(delay);

		UserSet userSet = GameController.instance.GetUserSet(userName);
		UserObject userObject = userSet.UserObject;
		UserState userState = userSet.UserState;

		if(GameController.instance.IsPlayer(userObject.Obj.name)) {
			GameObject result = userObject.Result;
			Text resultText = result.transform.FindChild("ResultText").GetComponent<Text>();
			string resultTimeText = ViewerController.instance.GetTimerText(userState.Record);
			if(ColorUtility.TryParseHtmlString(colorList["record"], out fontColor)) {
				ViewerController.instance.ChangeTextContent(resultText, messageList["time"], fontColor);
			}
			ViewerController.instance.ChangeRawImageState(result.GetComponent<RawImage>(), true);
			StartCoroutine(ViewerController.instance.ChangeTextState(resultText, true));
			SoundController.instance.ShotClipSound("record");
			StartCoroutine(AddCharacterContinuouslyForResult(resultText, resultTimeText.ToCharArray()));
		}
	}

	/// <summary>
	/// Adds the character continuously for result.
	/// </summary>
	/// <returns>The character continuously for result.</returns>
	/// <param name="resultText">Result text.</param>
	/// <param name="resultTimeTextArray">Result time text array.</param>
	private IEnumerator AddCharacterContinuouslyForResult(Text resultText, char[] resultTimeTextArray) {
		float clipLength = SoundController.instance.GetClipLength("record");

		foreach(char resultTimeText in resultTimeTextArray) {
			yield return new WaitForSeconds(clipLength);
			string newResultTimeText = resultText.text + resultTimeText;

			if(ColorUtility.TryParseHtmlString(colorList["result"], out fontColor)) {
				ViewerController.instance.ChangeTextContent(resultText, newResultTimeText, fontColor);
			}
			SoundController.instance.ShotClipSound("record");
		}
	}

}
