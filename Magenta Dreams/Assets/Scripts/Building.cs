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

    //Lars: Owner
    public enum Owner
    {
        NoOne,
        Player,
        AI
    }
    public Owner owner;

    //Lars:
    private AIManager aihq;

	// config
	private float connectionDuration = 30.0f; // Anschluss Arbeitsdauer
	private float errorDuration = 30.0f; // Fehlerbehebung Arbeitsdauer
	private float connectionMaxWaitingTime = 60.0f; // Maximale Wartezeit auf Techniker
	private float errorMaxWaitingTime = 30.0f; // Maximale Wartezeit auf Techniker

	// state
	private GameObject icon;
	public Status currentStatus = Status.Nothing;
	public float statusTimer = 0; // aktuelle Dauer
	public GameObject currentCar;

	// Use this for initialization
	void Start () {
		updateColor ();
        //Lars:
        aihq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<AIManager>();
        owner = Owner.NoOne;
	}

	// Update is called once per frame
	void Update () {
		if (currentStatus != Status.Nothing && currentStatus != Status.Connection) {
			statusTimer -= Time.deltaTime;

			// timer abgelaufen
			if (statusTimer < 0) {
				if (currentStatus == Status.ConnectionWait) {
					// wurde nicht angeschlossen
					currentStatus = Status.Nothing;
				} else if (currentStatus == Status.ConnectionProgress) {
					// fertig angeschlossen
					currentStatus = Status.Connection;
					currentCar.GetComponent<CarTargetSelect> ().EventFinished ();
				} else if (currentStatus == Status.ErrorWait) {
					// fehler wurde nicht behoben
					currentStatus = Status.Nothing;
				} else if (currentStatus == Status.ErrorProgress) {
					// fehler wurde behoben
					currentStatus = Status.Connection;
					currentCar.GetComponent<CarTargetSelect> ().EventFinished ();
				}
				// clear current car
				currentCar = null;
				statusTimer = 0;

				updateIcon ();
				updateColor ();
			}
		}
	}

	// Timer triggert diese Aktion
	// Checkt welche Eintritt
	public void SetAction() {
		if (currentStatus == Status.Nothing) {
			currentStatus = Status.ConnectionWait;
			statusTimer = connectionMaxWaitingTime;
            //Lars:
            aihq.AddEvent(this.gameObject);
		} else if (currentStatus == Status.Connection) {
			currentStatus = Status.ErrorWait;
			statusTimer = errorMaxWaitingTime;
            //Lars:
            if (owner == Owner.AI)
            {
                aihq.AddEvent(this.gameObject);
            }
		}
		updateIcon ();
		updateColor ();
	}

	private void updateIcon() {
		if (currentStatus != Status.Nothing && currentStatus != Status.Connection && !icon) {
			icon = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			icon.name = "Icon Waiting!";
			icon.transform.parent = gameObject.transform;
			icon.transform.position = gameObject.transform.position - new Vector3 (0, -(gameObject.transform.localScale.y * 2), 0);
			icon.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			icon.GetComponent<Renderer>().material.color = new Color(0,1,0,1);
		} else if (currentStatus == Status.Nothing || currentStatus == Status.Connection) {
			Destroy (icon);
		}
	}

	private void updateColor() {
		if (currentStatus == Status.Connection || currentStatus == Status.ErrorProgress || currentStatus == Status.ErrorWait) {
            //Lars:
            if (owner == Owner.Player)
                gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            else
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
        } else {
			gameObject.GetComponent<Renderer>().material.color = Color.grey;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        //Auto: Fahrt stoppen wenn zielobjekt erreicht
        //dort wird verglichen, ob es der Trigger vom Ziel des Autos war
        //- wenn ja: Stoppen
        //- wenn nein: weiterfahren
        //Lars:
        if ((other.CompareTag("Auto") && owner == Owner.Player) || (other.CompareTag("AIAuto") && owner == Owner.AI))
        {
			// Transform target = other.gameObject.GetComponent<AICharacterControl> ().Target;
			Transform target = other.gameObject.GetComponent<CarTargetSelect> ().target;
			// Dieses Gebäude Ziel?
			if (target && target == transform && currentCar == null) {
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
        //Lars:
        if ((other.CompareTag ("Auto") || other.CompareTag("AIAuto")) && other.gameObject == currentCar) {
			if (currentStatus == Status.ConnectionProgress) {
				currentStatus = Status.ConnectionWait;
			} else if (currentStatus == Status.ErrorProgress) {
				currentStatus = Status.ErrorWait;
			}
			currentCar = null;
		}
	}

	private void OnGUI()
	{
		if (currentStatus != Status.Nothing) {
			if (currentCar != null) {
				GUI.Label(new Rect(300, 0, 150, 50), "Aktives Objekt: " + currentCar.name);
			}
			GUI.Label(new Rect(300, 50, 150, 50), "Status: " + currentStatus);
			GUI.Label(new Rect(300, 100, 150, 50), "Status Timer: " + statusTimer.ToString());
		}
	}
}
