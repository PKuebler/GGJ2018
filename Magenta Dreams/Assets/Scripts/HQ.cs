using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HQ : MonoBehaviour 
{
	public GameObject carPrefab;
	public bool isPlayer; 

	public int cars;

	private int maxAICars = 5;
	private int difficultAI = 2;
	private int numbersOfCars = 1;
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		cars = carList.Count;
	}
		
	// Buy a Car -- int numbersOfCars
	public void BuyCars ()
	{
		if( (numbersOfCars * (carPrice * carList.Count)) <= money)
		{
			for(int i = 0; i < numbersOfCars; i++ )
			{
				Vector3 postition = new Vector3 (this.transform.position.x + (i + 1.0f), this.transform.position.y, this.transform.position.z);
				GameObject newCar = Instantiate (carPrefab, postition, rotation, this.transform);

				money = money - (carPrice * carList.Count);
				carList.Add (newCar);
			}
		}
	}

    public void AIBuyCars()
    {
		if (numbersOfCars * (carPrice * (GetComponent<AIManager> ().CarList.Count * difficultAI)) <= money && GetComponent<AIManager> ().CarList.Count <= maxAICars) 
		{
			Vector3 postition = new Vector3(this.transform.position.x + (1.0f), this.transform.position.y, this.transform.position.z);
			GameObject newCar = Instantiate(carPrefab, postition, rotation, this.transform);
			money = money - (carPrice * (GetComponent<AIManager> ().CarList.Count * difficultAI));
			GetComponent<AIManager>().AddCar(newCar);
		}
    }

//	void OnGUI ()
//	{
//		GUI.Label (new Rect (0, 100, 200, 50), "Funds: " + money.ToString () + "\nCars: " + carList.Count.ToString() );
//	}
		
	public void SetMoney(float newMoney)
	{
		money += Mathf.RoundToInt(newMoney);
	}
		
	// call from timer
	#region Erstellen der HQ Werte und erstes Auto
	public void InitStats()
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
		if (isPlayer != true) 
		{
			money = 0;
			Vector3 postition = new Vector3(this.transform.position.x + (1.0f), this.transform.position.y, this.transform.position.z);
			GameObject newCar = Instantiate(carPrefab, postition, rotation, this.transform);
			GetComponent<AIManager>().AddCar(newCar);
		} else 
		{
			money = 0;
			Vector3 postition = new Vector3 (this.transform.position.x + (0 + 1.0f), this.transform.position.y, this.transform.position.z);
			GameObject newCar = Instantiate (carPrefab, postition, rotation, this.transform);
			carList.Add (newCar);
		}

        GameObject.Find("GameManager").GetComponent<WinLose>().StartGameTimer();
	}
	#endregion
}
