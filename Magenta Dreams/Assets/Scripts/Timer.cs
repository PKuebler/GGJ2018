using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
	private GameObject[] childs;
	public float timeLeft = 30.0f;
	private float timeBetweenEvents = 10.0f;

	// Use this for initialization
	void Start () {
		childs = GameObject.FindGameObjectsWithTag ("Haus");


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

	// Get random Building
	void PickBuilding() {
		GameObject randomObject = childs[Random.Range(0,childs.Length - 1)];
		if (!randomObject) {
			PickBuilding ();
            return;
		}
		randomObject.GetComponent<Building>().SetAction ();
	}
}
