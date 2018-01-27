using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class AIManager : MonoBehaviour {

    [SerializeField]
    private List<CarTargetSelect> carList;
    [SerializeField]
    private List<GameObject> buildingsWithEvents;

    private float timer;
    private int money;
    private HQ aihq;


	// Use this for initialization
	void Awake () {
        carList = new List<CarTargetSelect>();
        buildingsWithEvents = new List<GameObject>();
        timer = 0.0f;
        aihq = GetComponent<HQ>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            if (buildingsWithEvents.Count > 0)
            {
                List<GameObject> errorList = new List<GameObject>();
                errorList = CheckForErrorWait(buildingsWithEvents);

                foreach (CarTargetSelect car in carList)
                {
                    if ((car.GetComponent<AICharacterControl>().Target == null && !car.Working) || car.GetComponent<AICharacterControl>().Target == this.transform)
                    {
                        GameObject closestTarget;
                        if (errorList.Count > 0)
                        {
                            closestTarget = GetDistance(errorList, car);
                        }
                        else
                        {
                            closestTarget = GetDistance(buildingsWithEvents, car);
                        }
                        car.GetComponent<AICharacterControl>().Target = closestTarget.GetComponent<Transform>();
                    }
                }
            }
            timer -= 1.0f;
        }

        if (aihq.Money > aihq.CarPrice)
        {
            Debug.Log("AI-Konto: " + aihq.Money.ToString());
            Debug.Log("Muahahaha, ein weiteres Auto!");
            aihq.AIBuyCars();
            Debug.Log("AI-Konto: " + aihq.Money.ToString());
        }
	}

    private List<GameObject> CheckForErrorWait(List<GameObject> buildings)
    {
        List<GameObject> errorList = new List<GameObject>();
        foreach (GameObject build in buildings)
        {
            if (build.GetComponent<Building>().currentStatus == Building.Status.ErrorWait)
            {
                errorList.Add(build);
            }
        }
        return errorList;
    }



    private GameObject GetDistance(List<GameObject> eventBuildings, CarTargetSelect car)
    {
        float[] distanceArray = new float[eventBuildings.Count];
        GameObject[] builds = new GameObject[eventBuildings.Count];

        int i = 0;

        foreach (GameObject eventBuilding in eventBuildings)
        {
            float temp = Vector3.Distance(eventBuilding.transform.position, car.transform.position);
            distanceArray[i] = temp;
            builds[i] = eventBuilding;
            i++;
        }
        int shortestIndex = MinValue(distanceArray);
        return builds[shortestIndex];
    }


    public void AddEvent (GameObject building)
    {
        buildingsWithEvents.Add(building);
    }

    public void RemoveEvent (GameObject building)
    {
        buildingsWithEvents.Remove(building);
        //Autos prüfen
    }

    public void AddCar (GameObject car)
    {
        carList.Add(car.GetComponent<CarTargetSelect>());
    }

    private int MinValue(float[] distances)
    {
        float min = 100.0f;
        int index = -1;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                index = i;
            }
        }
        return index;
    }
}
