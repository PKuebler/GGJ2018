using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
	public GameObject rootBuildings;
	private Transform[] childs;
	public float timeLeft = 0.0f;
	public float timeBetweenEvents = 30.0f;

	// Use this for initialization
	void Start () {
		childs = rootBuildings.GetComponentsInChildren<Transform>();
		PickBuilding();

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
	// if new
	// or
	// broken
	void PickBuilding() {
		Transform randomObject = childs[Random.Range(0,childs.Length)];
		print (randomObject.name);
		randomObject.GetComponent<Building>().SetWait (randomObject.name);
	}
}
