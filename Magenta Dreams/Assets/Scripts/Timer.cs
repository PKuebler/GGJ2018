using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
	private GameObject[] childs;
	public HQ player_hq;
	public HQ ai_hq;
	public GameObject SplashScreen;
	public GameObject PlayerUI;

	public float moneyTimeLeft = 20.0f;
	private float timeBetweenMoney = 5.0f;

	public float timeLeft = 30.0f;
	private float timeBetweenEvents = 15.0f;

	private bool isRun = false;
		
	// Use this for initialization
	void Start () {
		childs = GameObject.FindGameObjectsWithTag ("Haus");
		player_hq = GameObject.FindGameObjectWithTag ("HQ").GetComponent<HQ>();
		ai_hq = GameObject.FindGameObjectWithTag ("AIHQ").GetComponent<HQ>();

		// set start timer
		timeLeft = timeBetweenEvents;
	}

	// Update is called once per frame
	void Update () {
		if (!isRun) {
			return;
		}

		timeLeft -= Time.deltaTime;
		print (Time.deltaTime.ToString());
		if ( timeLeft < 0 ) {
			PickBuilding ();
			timeLeft = timeBetweenEvents;
		}
	}

	void FixedUpdate()
	{
		if (!isRun) {
			return;
		}

		moneyTimeLeft -= Time.deltaTime;
		if (moneyTimeLeft < 0) 
		{
			float moneyAI = 0;
			float moneyPlayer = 0;

			foreach (GameObject obj in childs) {
				Building building = obj.GetComponent<Building> ();

				moneyAI += building.moneyAI;
				moneyPlayer += building.moneyPlayer;

				building.moneyAI = 0;
				building.moneyPlayer = 0;
			}

			player_hq.SetMoney (moneyPlayer);
			ai_hq.SetMoney (moneyAI);
			moneyTimeLeft = timeBetweenMoney;
		}
	}

	// Get random Building
	void PickBuilding() {
		GameObject randomObject = childs[Random.Range(0,childs.Length - 1)];
		if (!randomObject) {
			PickBuilding ();
            return;
		}
		randomObject.GetComponent<Building>().SetEvent ();
	}

	// start game
	public void StartGame() {
		player_hq.InitStats ();
		ai_hq.InitStats ();
		isRun = true;
		SplashScreen.SetActive (false);
		PlayerUI.SetActive (true);
	}
}
