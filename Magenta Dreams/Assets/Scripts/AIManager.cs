using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public GameObject car;
    private List<GameObject> carList;
    public List<GameObject> BuildingsWithEvents { get; set; }

	// Use this for initialization
	void Start () {
        carList.Add(car);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void AddEvent (GameObject building)
    {
        BuildingsWithEvents.Add(building);
    }

    public void RemoveEvent (GameObject building)
    {
        BuildingsWithEvents.Remove(building);
    }

    public void AddCar (GameObject car)
    {

    }
}
