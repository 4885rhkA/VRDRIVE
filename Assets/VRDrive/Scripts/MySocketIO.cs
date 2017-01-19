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

	public IEnumerator SendMessage(string playerName) {
		string aaa = "{\"file\":\"" + DateTime.Now.ToString() + "\"}";
		JSONObject bbb = new JSONObject(aaa);
		socket.Emit("/file", bbb);
		while(!GameController.instance.FinishGameFlag) {
			UserSet userSet = GameController.instance.GetUserSet(playerName);
			UserObject userObject = userSet.UserObject;
			UserState userState = userSet.UserState;

			DateTime dtNow = DateTime.Now;
			TimeSpan tsNow = dtNow.TimeOfDay;
			float nowSpeed = userObject.Obj.GetComponent<MyCarController>().GetCurrentSpeed();
			Vector3 position = userObject.Obj.transform.position;
			float x = position.x;
			float z = position.z;
			Dictionary<string, bool> checkList = userState.CheckList;

			string msg = "{\"time\":\"" + tsNow.ToString() + "\",\"speed\":" + nowSpeed + ",\"x\":" + x + ",\"z\":" + z;

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
			yield return new WaitForSeconds(0.5f);
		}
	}

}
