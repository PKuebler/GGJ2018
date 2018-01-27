using UnityEngine;
using UnityEditor;
using System.Collections;

public class DebugOverview : EditorWindow
{
	private bool carsEnabled;
	private bool buildingsEnabled;
	private Vector2 scrollPos;

	[MenuItem ("Window/Debug Overview")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(DebugOverview));
	}

	void OnGUI () {
		EditorGUILayout.BeginVertical ();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		// cars
		carsEnabled = EditorGUILayout.Toggle("Show Cars", carsEnabled);

		if (carsEnabled)
			ShowCars ();

		buildingsEnabled = EditorGUILayout.Toggle("Show Buildings", buildingsEnabled);

		if (buildingsEnabled)
			ShowBuildings ();

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical ();
}

	private void ShowCars() {
		// The actual window code goes here
		GameObject[] carArray = GameObject.FindGameObjectsWithTag("Auto");

		for (int i = 0; i < carArray.Length; i++) {
			EditorGUILayout.LabelField("Car ", carArray[i].name);
		}
	}

	private void ShowBuildings() {
		GameObject[] buildingArray = GameObject.FindGameObjectsWithTag ("Haus");

		for (int i = 0; i < buildingArray.Length; i++) {
			Building b = buildingArray [i].GetComponent<Building> ();
			Building.Status lastStatus = b.currentStatus;

			EditorGUILayout.BeginHorizontal ();
			b.currentStatus = (Building.Status)EditorGUILayout.EnumPopup("[" + ((b.currentCar) ? "X" : " ") + "] " + buildingArray [i].name + " " + b.statusTimer + "s", b.currentStatus);

			if (lastStatus != b.currentStatus) {
				b.SetStatusToDebug();
			}

			b.isDebug = EditorGUILayout.Toggle("", b.isDebug, GUILayout.Width(10));
			EditorGUILayout.EndHorizontal ();
		}
	}

	void OnInspectorUpdate() {
		// Call Repaint on OnInspectorUpdate as it repaints the windows
		// less times as if it was OnGUI/Update
		Repaint();
	}
}
