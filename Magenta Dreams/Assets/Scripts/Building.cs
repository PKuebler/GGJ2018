using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

public class Building : MonoBehaviour {
	// Status
	public enum Status {
		Nothing, // Neutral
		ConnectionWait, // Warte auf Anschluss
		ConnectionProgress, // Techniker schließt an
		Connection, // Angeschlossen Neutral
		ErrorWait, // Fehler, warte auf Techniker
		ErrorProgress // Fehler, Techniker sitzt an behebung.
	}

	private GameObject icon;
	public Status currentStatus = Status.Nothing;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// Timer triggert diese Aktion
	// Checkt welche Eintritt
	public void SetAction() {
		if (currentStatus == Status.Nothing) {
			currentStatus = Status.ConnectionWait;
		} else if (currentStatus == Status.Connection) {
			currentStatus = Status.ErrorWait;
		}
		updateIcon ();
	}

	private void updateIcon() {
		if (currentStatus != Status.Nothing && currentStatus != Status.Connection && !icon) {
			icon = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			icon.name = "Icon Waiting!";
			icon.transform.parent = gameObject.transform;
			icon.transform.position = gameObject.transform.position - new Vector3 (0, -(gameObject.transform.localScale.y * 2), 0);
			icon.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		} else if (currentStatus == Status.Nothing || currentStatus == Status.Connection) {
			Destroy (icon);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        //Auto: Fahrt stoppen wenn zielobjekt erreicht
        //dort wird verglichen, ob es der Trigger vom Ziel des Autos war
        //- wenn ja: Stoppen
        //- wenn nein: weiterfahren
        if (other.CompareTag("Auto"))
        {
			Transform target = other.GetComponent<AICharacterControl> ().Target;
			// Dieses Gebäude Ziel?
			if (target && target == transform) {
				// Überhaupt bedarf?
				bool isCarWorking = false;

				if (currentStatus == Status.ConnectionWait) {
					currentStatus = Status.ConnectionProgress;
					isCarWorking = true;
				} else if (currentStatus == Status.ErrorWait) {
					currentStatus = Status.ErrorProgress;
					isCarWorking = true;
				}
				other.gameObject.GetComponent<CarTargetSelect>().ReachedTarget(this.gameObject, isCarWorking);
			}
        }
    }
}
