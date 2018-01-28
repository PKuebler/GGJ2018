using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SyncObjects : EditorWindow
{
	private GameObject root = null;
	private GameObject rootParent = null;
	private GameObject rootChild = null;
	private string button = "Auf alle Anwenden";
	private string removeButton = "Remove Clones";

	[MenuItem ("Window/SyncObjects")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(SyncObjects));
	}

	private Transform findChildByName(Transform main, string name) {
		for (int i = 0; i < main.childCount; i++) {
			Transform child = main.GetChild (i);
			if (child.name == name) {
				return child;
			}
		}
		return null;
	}

	void OnGUI() {
		root = (GameObject)EditorGUILayout.ObjectField ("GameObject mit allen Objekten", root, typeof(GameObject), true);
		rootParent = (GameObject)EditorGUILayout.ObjectField ("Parent Demo Objekt", rootParent, typeof(GameObject), true);
		rootChild = (GameObject)EditorGUILayout.ObjectField ("Child Demo Objekt (Muss dem Parent untergeordnet sein!)", rootChild, typeof(GameObject), true);

		if (!rootChild || !rootParent || !root) {
			return;
		}

		List<Transform> syncElements = new List<Transform>();

		// collect sync elements from root
		for (int i = 0; i < root.transform.childCount; i++) {
			syncElements.Add(root.transform.GetChild(i));
		}

		EditorGUILayout.LabelField(root.transform.childCount.ToString());

		if (syncElements.Count < 1) {
			return;
		}

		if (GUILayout.Button(button)) {
			foreach(Transform syncElement in syncElements) {
				if (syncElement == rootParent.transform) {
					continue;
				}

				// find clone children
				Transform clone = findChildByName(syncElement, "Clone Wars");

				if (!clone) {
					clone = ((GameObject)Instantiate (rootChild, new Vector3(0, 0, 0), Quaternion.identity)).transform;
					clone.gameObject.name = "Clone Wars";
					clone.parent = syncElement;
				}

				Transform tpl = rootChild.transform;
				clone.transform.localPosition = new Vector3 (tpl.localPosition.x, tpl.localPosition.y, tpl.localPosition.z);
				clone.transform.localRotation = new Quaternion(tpl.localRotation.x, tpl.localRotation.y, tpl.localRotation.z, tpl.localRotation.w);
				clone.transform.localScale = new Vector3 (tpl.localScale.x, tpl.localScale.y, tpl.localScale.z);
			}
			button = "Finish";
		}

		if (GUILayout.Button (removeButton)) {
			foreach (Transform syncElement in syncElements) {
				if (syncElement == rootParent.transform) {
					continue;
				}

				// find clone children
				Transform clone = findChildByName(syncElement, "Clone Wars");

				if (clone) {
					DestroyImmediate (clone.gameObject);
				}
			}
		}

		/*		if (GUILayout.Button (button)) {
			button = "Arbeite...";
			foreach (Transform child in children) {
				if (child == rootParent.transform) {
					continue;
				}

				Transform[] a = child.gameObject.GetComponentsInChildren<Transform> ();
				for (int i = 0; i < a.Length; i++) {
					Destroy (a[i]);
				}

				Transform clone = findChildByName (child);

				if (!clone) {
					clone = ((GameObject)Instantiate (rootChild, new Vector3(10.0F, 0, 0), Quaternion.identity)).transform;
					clone.gameObject.name = "Clone Wars";
					clone.parent = child;
				}

				clone.localPosition = rootChild.transform.localPosition;
				clone.localRotation = rootChild.transform.localRotation;
				clone.localScale = rootChild.transform.localScale;
			}
			button = "Auf alle Anwenden";
		}*/
	}
}
