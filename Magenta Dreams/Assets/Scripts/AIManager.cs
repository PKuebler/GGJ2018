using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public GameObject car;
    private List<GameObject> carList;
    private List<GameObject> buildingsWithEvents;

	// Use this for initialization
	void Start () {
        carList = new List<GameObject>();
        buildingsWithEvents = new List<GameObject>();

        Debug.Log(car);
        carList.Add(car);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void AddEvent (GameObject building)
    {
        buildingsWithEvents.Add(building);
    }

    public void RemoveEvent (GameObject building)
    {
        buildingsWithEvents.Remove(building);
    }

    public void AddCar (GameObject car)
    {

    }
}
