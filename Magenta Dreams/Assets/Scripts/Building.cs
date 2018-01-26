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

	// if auto fährt, vor ende

	// config
	private float connectionDuration = 30.f; // Anschluss Arbeitsdauer
	private float errorDuration = 30.f; // Fehlerbehebung Arbeitsdauer
	private float connectionMaxWaitingTime = 60.f; // Maximale Wartezeit auf Techniker
	private float errorMaxWaitingTime = 30.f; // Maximale Wartezeit auf Techniker

	// state
	private GameObject icon;
	public Status currentStatus = Status.Nothing;
	public float statusTimer = 0; // aktuelle Dauer
	public GameObject currentCar;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (currentStatus != Status.Nothing && currentStatus != Status.Connection) {
			statusTimer -= Time.deltaTime;

			if (statusTimer < 0) {
				if (currentStatus == Status.ConnectionWait) {
					// wurde nicht angeschlossen
					currentStatus = Status.Nothing;
				} else if (currentStatus == Status.ConnectionProgress) {
					// fertig angeschlossen
					currentStatus = Status.Connection;
				} else if (currentStatus == Status.ErrorWait) {
					// fehler wurde nicht behoben
					currentStatus = Status.Nothing;
				} else if (currentStatus == Status.ErrorProgress) {
					// fehler wurde behoben
					currentStatus = Status.Connection;
				}
				updateIcon ();
			}
		}
	}

	// Timer triggert diese Aktion
	// Checkt welche Eintritt
	public void SetAction() {
		if (currentStatus == Status.Nothing) {
			currentStatus = Status.ConnectionWait;
			statusTimer = connectionMaxWaitingTime;
		} else if (currentStatus == Status.Connection) {
			currentStatus = Status.ErrorWait;
			statusTimer = errorMaxWaitingTime;
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
					statusTimer = connectionDuration;
					isCarWorking = true;
					currentCar = other.gameObject;
				} else if (currentStatus == Status.ErrorWait) {
					currentStatus = Status.ErrorProgress;
					statusTimer = errorDuration;
					isCarWorking = true;
					currentCar = other.gameObject;
				}
				other.gameObject.GetComponent<CarTargetSelect>().ReachedTarget(this.gameObject, isCarWorking);
			}
        }
    }

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Auto") && other.gameObject == currentCar) {
			if (currentStatus == Status.ConnectionProgress) {
				currentStatus = Status.ConnectionWait;
			} else if (currentStatus == Status.ErrorProgress) {
				currentStatus = Status.ErrorWait;
			}
			currentCar = null;
		}
	}
}
