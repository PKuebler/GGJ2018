using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour {

	public GameObject carPrefab;
	private int money;
	private int carPrice;
	private GameObject[] buildingList;
	private GameObject[] carArray;
	private List<GameObject> carList;

	// Use this for initialization
	void Start () {
		money = 1000;
		carPrice = 100;
		// buildingList = GameObject.FindGameObjectsWithTag ("Haus");

		carList = new List<GameObject>();

		carArray = GameObject.FindGameObjectsWithTag("Auto"); 

		for (int i = 0; i < carArray.Length; i++) 
		{
			carList.Add(carArray[i]);
		}

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

				Vector3 postition = new Vector3 (this.transform.position.x + (i + 1.5f), this.transform.position.y, this.transform.position.z);
				Quaternion rotation = new Quaternion (0, 0, 0, 0);
				GameObject newCar = Instantiate (carPrefab, postition, rotation, this.transform);
				carList.Add (newCar);
				money = money - carPrice;
			}
		}
		else
		{
			// Throw Masage -> insufficient funds
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (0, 100, 100, 50), money.ToString ());
	}
}
