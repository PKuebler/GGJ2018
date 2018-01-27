using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    private GameObject[] carArray;
    private List<GameObject> carList;
    private GameObject[] buildingArray;
    public List<GameObject> BuildingsWithEvents { get; set; }

	// Use this for initialization
	void Start () {
        carArray = GameObject.FindGameObjectsWithTag("KIAuto");
        foreach (GameObject go in carArray)
        {
            carList.Add(go);
        }
        buildingArray = GameObject.FindGameObjectsWithTag("Haus");
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void AddEvent (GameObject building)
    {

    }

    public void RemoveEvent (GameObject building)
    {

    }
}
