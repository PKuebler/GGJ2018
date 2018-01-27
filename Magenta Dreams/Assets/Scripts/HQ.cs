using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour 
{

	public GameObject carPrefab;
	public GameObject buildingPrefab;
	private int money;
	private int carPrice;
	private GameObject[] buildingArray;
	private GameObject[] carArray;
	private List<GameObject> carList;
	private List<GameObject> buildingList;
	private Quaternion rotation;

	// Use this for initialization
	void Start () {

		InitStats ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	// Buy a Car -- int numbersOfCars
	public void BuyCars ()
	{
		int numbersOfCars = 1;
		if( (numbersOfCars * carPrice) <= money)
		{
			for(int i = 0; i < numbersOfCars; i++ )
			{
				Vector3 postition = new Vector3 (this.transform.position.x + (i + 1.0f), this.transform.position.y, this.transform.position.z);
				GameObject newCar = Instantiate (carPrefab, postition, rotation, this.transform);
				carList.Add (newCar);
				money = money - carPrice;
			}
		}
		else
		{
			// Throw Masage -> insufficient funds
			// DisplayDialog ("Title here", "Your text", "Ok");
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (0, 100, 200, 50), "Funds: " + money.ToString () + "\nCars: " + carList.Count.ToString() );
	}

	#region Erstellen der HQ Werte und erstes Auto
	void InitStats()
	{
		// HQ Stats
		money = 1100;
		carPrice = 100;

		rotation = new Quaternion (0, 0, 0, 0);

		// Setzen der Gebäude die bei Spielstart zum HQ gehören
		buildingList = new List<GameObject> ();
		buildingArray = GameObject.FindGameObjectsWithTag ("Haus");

		for (int i = 0; i < buildingArray.Length; i++) 
		{
			buildingList.Add(buildingArray[i]);
		}

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
