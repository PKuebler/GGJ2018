using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityStandardAssets.Characters.ThirdPerson;

public class HQ : MonoBehaviour 
{
	public GameObject carPrefab;
	public bool isPlayer; 

	private int contractPay;
	private int money;
	private int carPrice;
	private GameObject[] carArray;
	private List<GameObject> carList;
	private List<Building> buildingList;
	private Quaternion rotation;

	// Use this for initialization
	void Start () 
	{
		InitStats ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
		
	// Buy a Car -- int numbersOfCars
	public void BuyCars ()
	{
		int numbersOfCars = 1;
		GetMoney ();

		if( (numbersOfCars * carPrice) <= money)
		{
			for(int i = 0; i < numbersOfCars; i++ )
			{
				Vector3 postition = new Vector3 (this.transform.position.x + (i + 1.0f), this.transform.position.y, this.transform.position.z);
				GameObject newCar = Instantiate (carPrefab, postition, rotation, this.transform);
				if (isPlayer != true) 
				{
					//GetComponent<AIManager> ().AddCar (newCar);
				}
				carList.Add (newCar);
				money = money - carPrice;
			}
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (0, 100, 200, 50), "Funds: " + money.ToString () + "\nCars: " + carList.Count.ToString() );
	}

	public void AddBuilding(Building newBuilding)
	{
		buildingList.Add (newBuilding);
	}

	public void RemoveBuilding(Building removeBuilding)
	{

		foreach(Building building in buildingList)
		{
			if (GameObject.ReferenceEquals(removeBuilding, building))
			{
				buildingList.Remove (removeBuilding);
			}
		}
	}
		
	public void GetMoney()
	{
		foreach(Building building in buildingList)
		{
			if (building.currentStatus == Building.Status.Connection)
			{
					money = money + contractPay;
			}
		}
	}

	#region Trigger
	void OnTriggerEnter(Collider other)
	{
		//Auto: Fahrt stoppen wenn zielobjekt erreicht
		//dort wird verglichen, ob es der Trigger vom Ziel des Autos war
		//- wenn ja: Stoppen
		//- wenn nein: weiterfahren
		if (other.CompareTag("Auto"))
		{
			Transform target = other.gameObject.GetComponent<AICharacterControl> ().Target;
			// Dieses Gebäude Ziel?
			if (target && target == transform)
			{
				other.gameObject.GetComponent<CarTargetSelect>().ReachedTarget(this.gameObject, false);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Auto")) 
		{
			other.gameObject.GetComponent<CarTargetSelect>().EventFinished();
		}
	}
	#endregion 

		
	#region Erstellen der HQ Werte und erstes Auto
	void InitStats()
	{
		// HQ Stats
		money = 100;
		carPrice = 100;
		contractPay = 5;

		rotation = new Quaternion (0, 0, 0, 0);

		// Liste für Gebäude zu HQ
		buildingList = new List<Building> ();

		// Setzen der Autos die bei Spielstart zum HQ gehören
		carList = new List<GameObject>();
		carArray = GameObject.FindGameObjectsWithTag("Auto"); 

		for (int i = 0; i < carArray.Length; i++) 
		{
			carList.Add(carArray[i]);
		}
		// Kauf des ersten Autos
		BuyCars ();
	}
	#endregion
}
