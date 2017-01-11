using UnityEngine;
using System;
using System.Collections.Generic;

/// Class of the values for oprating parameters.
public class UserState {

	/// <list type="bullet">
	/// 	<item>
	/// 		<term>status</term>
	/// 		<description>By its value, controll the user.</description>
	/// 		<value>-1:standby / 0:nowplaying / 1:goal / 2:retire</value>
	/// 	</item>
	/// 	<item>
	/// 		<term>condition</term>
	/// 		<description>By its value, decide whether collision occurs or not.</description>
	/// 		<value>0:normal / 1:damage / 2:dash</value>
	/// 	</item>
	/// 	<item>
	/// 		<term>record</term>
	/// 		<description>The goal of missed record</description>
	/// 	</item>
	/// 	<item>
	/// 		<term>checkList</term>
	/// 		<description>Checklist for keeping rules</description>
	/// 	</item>
	/// </list>

	private int status;
	private int condition;
	private TimeSpan record;
	private Dictionary<string, bool> checkList;

	public UserState() {
		status = -1;
		condition = 0;
		record = new TimeSpan(0, 0, 0);
		checkList = new Dictionary<string, bool>();
	}

	public int Status {
		get {
			return status;
		}
		set {
			status = value;
		}
	}

	public int Condition {
		get {
			return condition;
		}
		set {
			condition = value;
		}
	}

	public TimeSpan Record {
		get {
			return record;
		}
		set {
			record = value;
		}
	}

	public Dictionary<string, bool> CheckList {
		get {
			return checkList;
		}
		set {
			checkList = value;
		}
	}

}
