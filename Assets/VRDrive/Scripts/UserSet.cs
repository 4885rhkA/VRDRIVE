using UnityEngine;
using System.Collections;

/// <summary>
/// User set.
/// </summary>
public class UserSet {

	private UserObject userObject;
	private UserState userState;

	public UserSet(UserObject userObject, UserState userState) {
		this.userObject = userObject;
		this.userState = userState;
	}

	public UserObject UserObject {
		get {
			return userObject;
		}
	}

	public UserState UserState {
		get {
			return userState;
		}
	}

}
