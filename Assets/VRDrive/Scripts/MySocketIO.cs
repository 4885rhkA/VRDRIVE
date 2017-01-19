using System.Collections;
using UnityEngine;
using SocketIO;
using System;

public class MySocketIO : MonoBehaviour {
	private SocketIOComponent socket;

	void Start() {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
	}

	public IEnumerator SendMessage(string name, float speed, Vector3 position) {
		while(!GameController.instance.FinishGameFlag) {
			DateTime dtNow = DateTime.Now;
			TimeSpan tsNow = dtNow.TimeOfDay;
			float x = position.x;
			float z = position.z;
			string msg = "{\"time\":\"" + tsNow.ToString() + "\",\"speed\":" + speed + ",\"x\":" + x + ",\"z\":" + z + "}";
			JSONObject json = new JSONObject(msg);
			socket.Emit("/car", json);
			yield return new WaitForSeconds(1f);
		}
	}

}
