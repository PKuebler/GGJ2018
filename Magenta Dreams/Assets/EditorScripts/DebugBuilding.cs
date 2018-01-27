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
			switch (myTarget.currentStatus) {
			case Building.Status.Nothing:
				myTarget.SetStatusNothing();
				break;
			case Building.Status.ConnectionWait:
				myTarget.SetStatusConnectionWait();
				break;
			case Building.Status.ConnectionProgress:
				// no car
				break;
			case Building.Status.Connection:
				myTarget.SetStatusConnection ();
				break;
			case Building.Status.ErrorWait:
				myTarget.SetStatusErrorWait ();
				break;
			case Building.Status.ErrorProgress:
				// no car
				break;
			}
		}

		EditorGUILayout.LabelField("Timer", myTarget.statusTimer.ToString() + "s");
		EditorGUILayout.LabelField("Current Car", (myTarget.currentCar) ? myTarget.currentCar.name : "No Car");
	}
}