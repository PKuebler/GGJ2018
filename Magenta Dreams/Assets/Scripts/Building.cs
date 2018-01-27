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

	// Cars At Building Colider
	public List<GameObject> TriggerCars = new List<GameObject>();

/*
//The list of colliders currently inside the trigger
 var TriggerList : List.<Collider> = new List.<Collider>();
 
 //called when something enters the trigger
 function OnTriggerEnter(other : Collider)
 {
     //if the object is not already in the list
     if(!TriggerList.Contains(other))
     {
         //add the object to the list
         TriggerList.Add(Other);
     }
 }
 
 //called when something exits the trigger
 function OnTriggerExit(other : Collider)
 {
     //if the object is in the list
     if(TriggerList.Contains(other))
     {
         //remove it from the list
         TriggerList.Remove(Other);
     }
 }*/

	// Use this for initialization
	void Start () {
		updateColor ();
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
					SetStatusNothing ();
				} else if (currentStatus == Status.ConnectionProgress) {
					// fertig angeschlossen
					owner = (currentCar.tag == "Auto")? Owner.Player : Owner.AI;
					SetStatusConnection();
				} else if (currentStatus == Status.ErrorWait) {
					// fehler wurde nicht behoben
					SetStatusNothing();
				} else if (currentStatus == Status.ErrorProgress) {
					// fehler wurde behoben
					SetStatusConnection();
				}
			}
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

    void OnTriggerEnter(Collider other)
    {
    	// only cars
    	if (!other.CompareTag("Auto") && !other.CompareTag("AIAuto")) {
    		return;
    	}

    	GameObject car = other.gameObject;

		if (!TriggerCars.Contains(car)) {
			TriggerCars.Add(car);
		}

        //Auto: Fahrt stoppen wenn zielobjekt erreicht
        //dort wird verglichen, ob es der Trigger vom Ziel des Autos war
        //- wenn ja: Stoppen
        //- wenn nein: weiterfahren
		if (other.CompareTag("Auto") || other.CompareTag("AIAuto"))
        {
			// nur eigene gebäude bei error
			if (currentStatus == Status.ErrorWait) {
				if (other.CompareTag ("Auto") && owner != Owner.Player) {
					// ignore event
					return;
				} else if (other.CompareTag ("AIAuto") && owner != Owner.AI) {
					// ignore event
					return;
				}
			}

			// Transform target = other.gameObject.GetComponent<AICharacterControl> ().Target;
			Transform target = other.gameObject.GetComponent<CarTargetSelect> ().target;
			// Dieses Gebäude Ziel?
			if (target && target == transform && currentCar == null) {
				// Überhaupt bedarf?
				bool isCarWorking = false;

				if (currentStatus == Status.ConnectionWait) {
					SetStatusConnectionProgress (other.gameObject);
					isCarWorking = true;
				} else if (currentStatus == Status.ErrorWait) {
					SetStatusErrorProgress (other.gameObject);
					isCarWorking = true;
				}
				other.gameObject.GetComponent<CarTargetSelect> ().ReachedTarget (this.gameObject, isCarWorking);
			}
        }
    }

	void OnTriggerExit(Collider other)
	{
    	// only cars
    	if (!other.CompareTag("Auto") && !other.CompareTag("AIAuto")) {
    		return;
    	}

    	GameObject car = other.gameObject;

		if (TriggerCars.Contains(car)) {
			TriggerCars.Remove(car);
		}

        if ((other.CompareTag ("Auto") || other.CompareTag("AIAuto")) && other.gameObject == currentCar) {
			if (currentStatus == Status.ConnectionProgress) {
				SetStatusConnectionWait ();
			} else if (currentStatus == Status.ErrorProgress) {
				SetStatusErrorWait ();
			}
		}
	}

	public void SetStatusToDebug() {
		if (aihq) {
			aihq.RemoveEvent(gameObject);
		}
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
		statusTimer = 0;
		currentCar = null;
		currentStatus = Status.Nothing;

		UpdateUI ();
	}

	public void SetStatusConnectionWait() {
		statusTimer = connectionMaxWaitingTime;
		currentCar = null;
		currentStatus = Status.ConnectionWait;
		if (aihq) {
			aihq.AddEvent (gameObject);
		}

		UpdateUI ();
	}

	public void SetStatusConnectionProgress(GameObject car) {
		statusTimer = connectionDuration;
		currentStatus = Status.ConnectionProgress;
		currentCar = car;

		UpdateUI ();
	}

	public void SetStatusConnection() {
		statusTimer = 0;
		if (currentCar) {
			currentCar.GetComponent<CarTargetSelect> ().EventFinished ();
		}
		currentCar = null;
		currentStatus = Status.Connection;
		UpdateUI ();
	}

	public void SetStatusErrorWait() {
		statusTimer = errorMaxWaitingTime;
		currentCar = null;
		currentStatus = Status.ErrorWait;
		if (owner == Owner.AI && aihq)
		{
			aihq.AddEvent(gameObject);
		}
		UpdateUI ();
	}

	public void SetStatusErrorProgress(GameObject car) {
		statusTimer = errorDuration;
		currentStatus = Status.ErrorProgress;
		currentCar = car;

		UpdateUI ();
	}

	private void UpdateUI() {
		updateIcon ();
		updateColor ();
	}
}
