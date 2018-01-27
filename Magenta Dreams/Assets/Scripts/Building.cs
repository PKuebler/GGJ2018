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

    public enum Owner
    {
        NoOne,
        Player,
        AI
    }
    public Owner owner;

    private AIManager aihq;

	// config
	private float connectionDuration = 15.0f; // Anschluss Arbeitsdauer
	private float errorDuration = 15.0f; // Fehlerbehebung Arbeitsdauer
	private float connectionMaxWaitingTime = 40.0f; // Maximale Wartezeit auf Techniker
	private float errorMaxWaitingTime = 20.0f; // Maximale Wartezeit auf Techniker

	// state
	private GameObject icon;
	public Status currentStatus = Status.Nothing;
	public float statusTimer = 0; // aktuelle Dauer
	public GameObject currentCar;
	public bool isDebug = false; // log debug

	// collect money
	public float moneyAI = 0;
	public float moneyPlayer = 0;

	// Use this for initialization
	void Start () {
		UpdateUI ();
        aihq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<AIManager>();
        owner = Owner.NoOne;
	}

	// Auto Checkin
	// true => Arbeitet schon hier, arbeitet jetzt hier
	// false => Besetzt
	public bool CheckIn(GameObject car) {
        // only cars
        Debug.Log("auto eingecheckt");
        if (!car.CompareTag("Auto") && !car.CompareTag("AIAuto")) {
			debugMessage (car, "CheckIn", "Heute nur Autos!");
			return false;
		}

		debugMessage (car, "CheckIn", "Hallo, lass mich deine Daten überprüfen!");

		// arbeitet dieses auto hier schon?
		if (currentCar == car) {
			debugMessage (car, "CheckIn", "Du arbeitest doch gerade schon hier!");
			return true;
		}

		debugMessage (car, "CheckIn", "Du arbeitest hier noch nicht!");

		// wartet haus auf ein auto?
		if (currentStatus != Status.ConnectionWait && currentStatus != Status.ErrorWait) {
			debugMessage (car, "CheckIn", "Zum Glück gerade keine Probleme... Sry.");
			return false;
		}

		debugMessage (car, "CheckIn", "Gut das du kommst, hier brennt die Hütte!");

		// gehört das haus evtl einem anderen spieler?
		if (owner != Owner.NoOne && owner != (car.tag == "AIAuto" ? Owner.AI : Owner.Player)) {
			// informiere auto, das dieses haus wem anders gehört
			debugMessage (car, "CheckIn", "Dieses Haus gehört leider einem anderen Team!");
			return false;
		}

		debugMessage (car, "CheckIn", "Dieses Haus gehört deinem Team oder niemandem!");

		// Fange an zu arbeiten!
		if (currentStatus == Status.ConnectionWait) {
			SetStatusConnectionProgress (car);
		} else if (currentStatus == Status.ErrorWait) {
			SetStatusErrorProgress(car);
		}

		// sage auto bescheid
		debugMessage (car, "CheckIn", "Auto beginnt mit der arbeit!");
		return true;
	}

	// Auto Checkout
	public bool CheckOut(GameObject car) {
		// only cars
		if (!car.CompareTag("Auto") && !car.CompareTag("AIAuto")) {
			debugMessage (car, "CheckOut", "Heute nur Autos!");
			return false;
		}

		debugMessage (car, "CheckOut", "Auto möchte auschecken!");

		// arbeitet dieses auto hier überhaupt?
		if (currentCar != car) {
			debugMessage (car, "CheckOut", "Dieses Auto hat hier nicht gearbeitet!");
			return false;
		}

		debugMessage (car, "CheckOut", "Auto hat hier gearbeitet!");

		// update haus prozess, currentCar wird automatisch auf null gesetzt
		if (currentStatus == Status.ConnectionProgress) {
			SetStatusConnectionWait ();
		} else if (currentStatus == Status.ErrorProgress) {
			SetStatusErrorWait ();
		}

		debugMessage (car, "CheckOut", "Im Haus arbeitet nun keiner mehr");
		return true;
	}

	// Update is called once per frame
	void Update () {
		// geld
		if (currentStatus == Status.Connection) {
			if (owner == Owner.AI) {
				moneyAI += Time.deltaTime;
			} else if (owner == Owner.Player) {
				moneyPlayer += Time.deltaTime;
			}
		}

		// wartet auf nix?
		if (currentStatus == Status.Nothing || currentStatus == Status.Connection) {
			return;
		}

		// update timer, nie unter 0
		statusTimer = Mathf.Max(statusTimer - Time.deltaTime, 0);

		// timer noch nicht abgelaufen
		if (statusTimer > 0) {
			return;
		}

		debugMessage (null, "Update", "Timer `" + currentStatus + "` abgelaufen.");

		if (currentStatus == Status.ConnectionWait) {
			// wurde nicht angeschlossen
			SetStatusNothing ();
		} else if (currentStatus == Status.ConnectionProgress) {
			// fertig angeschlossen

			// setze eigentümer
			owner = (currentCar.tag == "Auto")? Owner.Player : Owner.AI;
			debugMessage (null, "Update", "Neuer Eigentümer `" + owner + "`.");

			SetStatusConnection();
		} else if (currentStatus == Status.ErrorWait) {
			// fehler wurde nicht behoben
			SetStatusNothing();
		} else if (currentStatus == Status.ErrorProgress) {
			// fehler wurde behoben
			SetStatusConnection();
		}
	}

	// Timer triggert diese Aktion
	// Checkt welche Eintritt
	public void SetEvent() {
		if (currentStatus == Status.Nothing) {
			SetStatusConnectionWait ();
		} else if (currentStatus == Status.Connection) {
			SetStatusErrorWait ();
		}
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
            if (owner == Owner.Player)
                gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            else
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
        } else {
			gameObject.GetComponent<Renderer>().material.color = Color.grey;
		}
	}

	public void SetStatusToDebug() {
		switch (currentStatus) {
			case Status.Nothing:
				SetStatusNothing();
				break;
			case Status.ConnectionWait:
				SetStatusConnectionWait();
				break;
			case Status.ConnectionProgress:
				// no car
				break;
			case Status.Connection:
				owner = Owner.Player;
				SetStatusConnection ();
				break;
			case Status.ErrorWait:
				SetStatusErrorWait ();
				break;
			case Status.ErrorProgress:
				// no car
				break;
		}
	}

	public void SetStatusNothing() {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		statusTimer = 0;
		currentCar = null;
		currentStatus = Status.Nothing;

		// informiere ki das dieses objekt keinen techniker mehr braucht
		if (aihq) {
			aihq.RemoveEvent (gameObject);
		}

		debugMessage (null, "SetStatusNothing", "Updated");
		UpdateUI ();
	}

	public void SetStatusConnectionWait() {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		statusTimer = connectionMaxWaitingTime;
		currentCar = null;
		currentStatus = Status.ConnectionWait;

		// informiere ki das dieses objekt einen techniker braucht
		if (aihq) {
			aihq.AddEvent (gameObject);
		}

		debugMessage (null, "SetStatusConnectionWait", "Updated");
		UpdateUI ();
	}

	public void SetStatusConnectionProgress(GameObject car) {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		statusTimer = connectionDuration;
		currentStatus = Status.ConnectionProgress;

		// neues auto
		currentCar = car;

		// informiere ki das dieses objekt keinen techniker mehr braucht
		if (aihq) {
			aihq.RemoveEvent (gameObject);
		}

		debugMessage (car, "SetStatusConnectionProgress", "Updated");
		UpdateUI ();
	}

	public void SetStatusConnection() {
		statusTimer = 0;

		// informiere auto, das es fertig ist!
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		debugMessage (currentCar, "SetStatusConnection", "Updated");

		currentCar = null;
		currentStatus = Status.Connection;

		UpdateUI ();
	}

	public void SetStatusErrorWait() {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		statusTimer = errorMaxWaitingTime;
		currentCar = null;
		currentStatus = Status.ErrorWait;

		// informiere ki, das ein techniker benötigt wird, falls das gebäude der ki gehört.
		if (aihq && owner == Owner.AI) {
			aihq.AddEvent(gameObject);
		}

		debugMessage (null, "SetStatusErrorWait", "Updated");

		UpdateUI ();
	}

	public void SetStatusErrorProgress(GameObject car) {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect>().EventComplete ();
		}

		statusTimer = errorDuration;
		currentStatus = Status.ErrorProgress;
		// neues auto
		currentCar = car;

		// informiere ki das dieses objekt keinen techniker mehr braucht
		if (aihq) {
			aihq.RemoveEvent (gameObject);
		}

		debugMessage (currentCar, "SetStatusErrorProgress", "Updated");

		UpdateUI ();
	}

	private void UpdateUI() {
		updateIcon ();
		updateColor ();
	}

	private void debugMessage(GameObject car, string action, string msg) {
		if (isDebug) {
			print ("[" + gameObject.name + "]" + ((car) ? "[" + car.name + "]" : "") + "[" + action + "] " + msg);
		}
	}
}
