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

	public int cars;

	private int contractPay;
    [SerializeField]
	private int money;
    public int Money { get { return money; } }
	private int carPrice;
    public int CarPrice { get { return carPrice; } }
	private GameObject[] carArray;
	public List<GameObject> carList;
	private Quaternion rotation;

	// Use this for initialization
	void Start () 
	{
		InitStats ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		cars = carList.Count;
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

				if (isPlayer != true) {
					GetComponent<AIManager> ().AddCar (newCar);
				} else {
					carList.Add (newCar);
					money = money - carPrice;
				}

			}
		}
	}

    public void AIBuyCars()
    {
        Vector3 postition = new Vector3(this.transform.position.x + (1.0f), this.transform.position.y, this.transform.position.z);
        GameObject newCar = Instantiate(carPrefab, postition, rotation, this.transform);
        GetComponent<AIManager>().AddCar(newCar);
        money = money - carPrice;
    }

	void OnGUI ()
	{
		GUI.Label (new Rect (0, 100, 200, 50), "Funds: " + money.ToString () + "\nCars: " + carList.Count.ToString() );
	}
		
	public void SetMoney(float newMoney)
	{
		money += Mathf.RoundToInt(newMoney);
	}
		
	#region Erstellen der HQ Werte und erstes Auto
	void InitStats()
	{
		// HQ Stats
		money = 100;
		carPrice = 100;
		contractPay = 5;

		rotation = new Quaternion (0, 0, 0, 0);

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
