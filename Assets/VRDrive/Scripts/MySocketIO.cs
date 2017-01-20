using System.Collections;
using UnityEngine;
using SocketIO;
using System;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;


public class MySocketIO : MonoBehaviour {
	private SocketIOComponent socket;

	void Start() {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
	}

	public void SendStop() {
		socket.Emit("disconnect");
	}

	public IEnumerator SendMessage(string playerName) {
		string aaa = "{\"file\":\"" + DateTime.Now.ToString() + "\"}";
		JSONObject bbb = new JSONObject(aaa);
		socket.Emit("/init", bbb);

		string ccc = "{\"speed\",\"x\",\"z\"";
		UserSet ddd = GameController.instance.GetUserSet(playerName);
		UserObject obj = ddd.UserObject;
		UserState state = ddd.UserState;
		Dictionary<string, bool> checks = state.CheckList;
		foreach(KeyValuePair<string, bool> pair in checks) {
			string key = "check-" + pair.Key;
			string temp = ",\"" + key + "\"";
			ccc = ccc + temp;
		}
		ccc = ccc + "}";
		JSONObject eee = new JSONObject(ccc);
		socket.Emit("/items", eee);

		while(!GameController.instance.FinishGameFlag) {
			UserSet userSet = GameController.instance.GetUserSet(playerName);
			UserObject userObject = userSet.UserObject;
			UserState userState = userSet.UserState;

			float nowSpeed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();
			Vector3 position = userObject.Obj.transform.position;
			float x = position.x;
			float z = position.z;
			Dictionary<string, bool> checkList = userState.CheckList;

			string msg = "{\"time\":\"" + DateTime.Now.ToString("hh:mm:ss")
			             + "\",\"speed\":" + nowSpeed + ",\"x\":" + x + ",\"z\":" + z;

			foreach(KeyValuePair<string, bool> pair in checkList) {
				string key = "check-" + pair.Key;
				int value = 0;
				if(pair.Value) {
					value = 1;
				}
				string temp = ",\"" + key + "\":" + value;
				msg = msg + temp;
			}
			msg = msg + "}";

			JSONObject json = new JSONObject(msg);
			socket.Emit("/car", json);
			yield return new WaitForSeconds(1f);
		}
	}

}
