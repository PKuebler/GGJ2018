﻿#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Building))]
public class LevelScriptEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		// todo: buildings -> reducer's die das object ändern und getriggert werden von debug und building
		Building myTarget = (Building)target;

		Building.Status lastStatus = myTarget.currentStatus;
		myTarget.currentStatus = (Building.Status)EditorGUILayout.EnumPopup("Status:", myTarget.currentStatus);

		if (lastStatus != myTarget.currentStatus) {
			myTarget.SetStatusToDebug();
		}

		myTarget.isDebug = EditorGUILayout.Toggle("Debug Modus", myTarget.isDebug);

		EditorGUILayout.LabelField("Timer", myTarget.statusTimer.ToString() + "s");
		EditorGUILayout.LabelField("Current Car", (myTarget.currentCar) ? myTarget.currentCar.name : "No Car");
		EditorGUILayout.LabelField("Collected Money");
		EditorGUILayout.LabelField("Player", myTarget.moneyPlayer.ToString());
		EditorGUILayout.LabelField("AI", myTarget.moneyAI.ToString());
	}
}

#endif