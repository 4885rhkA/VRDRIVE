using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ValueKeeper : MonoBehaviour {

	private Dictionary<string, UserState> userStateList = new Dictionary<string, UserState>();

	public Dictionary<string, UserState> UserStateList {
		get {
			return userStateList;
		}
		set {
			userStateList = value;
		}
	}

}
