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
	private float connectionDuration = 30.0f; // Anschluss Arbeitsdauer
	private float errorDuration = 30.0f; // Fehlerbehebung Arbeitsdauer
	private float connectionMaxWaitingTime = 60.0f; // Maximale Wartezeit auf Techniker
	private float errorMaxWaitingTime = 30.0f; // Maximale Wartezeit auf Techniker

	// state
	private GameObject icon;
	public Status currentStatus = Status.Nothing;
	public float statusTimer = 0; // aktuelle Dauer
	public GameObject currentCar;
	public bool isDebug = false; // log debug

	// Cars At Building Colider
	public List<GameObject> TriggerCars = new List<GameObject>();

	// Use this for initialization
	void Start () {
		UpdateUI ();
        aihq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<AIManager>();
        owner = Owner.NoOne;
	}

	// Update is called once per frame
	void Update () {
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
	public void SetAction() {
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

	// objekt betritt den collider
    void OnTriggerEnter(Collider other)
    {
		// only cars
		if (!other.CompareTag("Auto") && !other.CompareTag("AIAuto")) {
			return;
		}

		GameObject car = other.gameObject;
		// füge auto in liste von autos im collider
		if (!TriggerCars.Contains(car)) {
			TriggerCars.Add(car);
		}

		debugMessage (car, "OnEnter", "enter");

		CheckTriggerEnter (car);
    }

	// wird beim austritt des aktuellen workers gegen alle im collider gefahren um zu schauen ob wer wartet
	private bool CheckTriggerEnter(GameObject car) {
		// arbeitet dieses auto hier schon?
		if (currentCar == car) {
			debugMessage (car, "CheckTrigger", "auto arbeitet hier bereits!");
			return true;
		}

		debugMessage (car, "CheckTrigger", "auto arbeitet hier noch nicht!");

		// get target from car
		Transform target = car.GetComponent<CarTargetSelect> ().target;

		// gibt es kein ziel oder ist das ziel nicht dieses gebäude?
		if (!target || target != transform) {
			debugMessage (car, "CheckTrigger", "auto hat kein oder ein anderes ziel!");
			return false;
		}

		debugMessage (car, "CheckTrigger", "auto hat dieses ziel!");

		// wartet haus auf ein auto?
		if (currentStatus != Status.ConnectionWait && currentStatus != Status.ErrorWait) {
			// informiere auto, das es hier nicht gebraucht wird
			car.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);

			debugMessage (car, "CheckTrigger", "warte nicht auf auto, auto wurde darüber informiert!");
			return false;
		}

		debugMessage (car, "CheckTrigger", "auto kann hier arbeiten!");

		// gehört das haus evtl einem anderen spieler?
		if (owner != Owner.NoOne && owner != (car.tag == "AIAuto" ? Owner.AI : Owner.Player)) {
			// informiere auto, das dieses haus wem anders gehört
			car.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);

			debugMessage (car, "CheckTrigger", "auto vom falschen team!");
			return false;
		}

		debugMessage (car, "CheckTrigger", "auto ist willkommen!");

		// auto will zu diesem gebäude, das gebäude wartet noch und alles andere ist auch geil
		if (currentStatus == Status.ConnectionWait) {
			SetStatusConnectionProgress (car);
		} else if (currentStatus == Status.ErrorWait) {
			SetStatusErrorProgress(car);
		}

		// sage auto bescheid
		car.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, true);

		debugMessage (car, "CheckTrigger", "auto ist informiert!");

		return true;
	}

	void OnTriggerExit(Collider other)
	{
    	// only cars
    	if (!other.CompareTag("Auto") && !other.CompareTag("AIAuto")) {
    		return;
    	}

    	GameObject car = other.gameObject;

		// entferne auto aus liste von autos im collider
		if (TriggerCars.Contains(car)) {
			TriggerCars.Remove(car);
		}

		debugMessage (car, "OnExit", "auto fährt =(");

		// arbeitet dieses auto hier überhaupt?
		if (currentCar != car) {
			debugMessage (car, "OnExit", "auto hat hier nicht gearbeitet!");
			return;
		}

		debugMessage (car, "OnExit", "auto hat hier gearbeitet!");

		// wird dieses haus überhaupt gerade von einem techniker versorgt?
		if (currentStatus != Status.ConnectionProgress && currentStatus != Status.ErrorProgress) {
			debugMessage (car, "OnExit", "haus brauchte keinen arbeiter");
			return;
		}

		debugMessage (car, "OnExit", "haus brauchte arbeiter");

		// update haus prozess
		if (currentStatus == Status.ConnectionProgress) {
			SetStatusConnectionWait ();
		} else if (currentStatus == Status.ErrorProgress) {
			SetStatusErrorWait ();
		}

		debugMessage (car, "OnExit", "haus arbeitet nun nicht mehr");

		// ist ein anderes auto in der nähe, das hier arbeiten könnte?
		foreach(GameObject otherCar in TriggerCars) {
			// eins gefunden?
			if (CheckTriggerEnter (otherCar)) {
				debugMessage (car, "OnExit", "anderes auto übernimmt: " + otherCar.name);
				return;
			}
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
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
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
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
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
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
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
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
		}

		debugMessage (currentCar, "SetStatusConnection", "Updated");

		currentCar = null;
		currentStatus = Status.Connection;

		UpdateUI ();
	}

	public void SetStatusErrorWait() {
		// falls auto arbeitete, informiere es, das es nicht mehr benötigt wird.
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
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
			currentCar.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, false);
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
