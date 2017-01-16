using System.Collections;
using UnityEngine;
using SocketIO;

public class MySocketIO : MonoBehaviour {
	private SocketIOComponent socket;

	public void Start() {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", OpenSocketIO);
		socket.On("error", ErrorSocketIO);
		socket.On("close", CloseSocketIO);

		socket.On("padstate", PadState);

		StartCoroutine("BeepBoop");
	}

	private void PadState(SocketIOEvent e) {
		Debug.Log(e.data);
	}

	public void OpenSocketIO(SocketIOEvent e) {
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void ErrorSocketIO(SocketIOEvent e) {
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}

	public void CloseSocketIO(SocketIOEvent e) {	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
