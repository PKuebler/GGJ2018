using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
	private GameObject[] childs;
	public HQ player_hq;
	public HQ ai_hq;

	public float moneyTimeLeft = 40.0f;
	private float timeBetweenMoney = 20.0f;

	public float timeLeft = 30.0f;
	private float timeBetweenEvents = 10.0f;
		
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
		timeLeft -= Time.deltaTime;
		if ( timeLeft < 0 ) {
			PickBuilding ();
			timeLeft = timeBetweenEvents;
		}
	}

	void FixedUpdate()
	{
		moneyTimeLeft -= Time.deltaTime;
		if (moneyTimeLeft < 0) 
		{
			player_hq.GetMoney ();
			//ai_hq.GetMoney ();
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
}
