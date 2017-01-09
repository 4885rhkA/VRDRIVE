using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ValueKeeper : MonoBehaviour {

	private Dictionary<string, UserSet> userSetList = new Dictionary<string, UserSet>();

	public Dictionary<string, UserSet> UserSetList {
		get {
			return userSetList;
		}
		set {
			userSetList = value;
		}
	}

}
