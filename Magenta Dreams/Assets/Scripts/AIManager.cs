using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIManager : MonoBehaviour {

    [SerializeField]
    private List<CarTargetSelect> carList;
    [SerializeField]
    private List<GameObject> buildingsWithEvents;

	// Use this for initialization
	void Awake () {
        carList = new List<CarTargetSelect>();
        buildingsWithEvents = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (buildingsWithEvents.Count > 0)
        {
            foreach (CarTargetSelect car in carList)
            {
                if (car.GetComponent<AICharacterControl>().Target == null || car.GetComponent<AICharacterControl>().Target == this.transform)
                {
                    car.GetComponent<AICharacterControl>().Target = buildingsWithEvents[0].transform;
                    car.target = buildingsWithEvents[0].transform;
                }
            }
        }
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
        carList.Add(car.GetComponent<CarTargetSelect>());
    }
}
