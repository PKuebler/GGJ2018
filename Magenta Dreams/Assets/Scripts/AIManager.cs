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


	// Use this for initialization
	void Awake () {
        carList = new List<CarTargetSelect>();
        buildingsWithEvents = new List<GameObject>();
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            if (buildingsWithEvents.Count > 0)
            {
                foreach (CarTargetSelect car in carList)
                {
                    if ((car.GetComponent<AICharacterControl>().Target == null && !car.Working) || car.GetComponent<AICharacterControl>().Target == this.transform)
                    {
                        Transform closestTarget = GetDistance(buildingsWithEvents, car);
                        car.GetComponent<AICharacterControl>().Target = closestTarget;
                    }
                }
            }
            timer -= 1.0f;
        }
        
	}



    private Transform GetDistance(List<GameObject> eventBuildings, CarTargetSelect car)
    {
        float[] distanceArray = new float[eventBuildings.Count];
        GameObject[] builds = new GameObject[eventBuildings.Count];

        int i = 0;

        foreach (GameObject eventBuilding in eventBuildings)
        {
            float temp = Vector3.Distance(eventBuilding.transform.position, car.transform.position);
            Debug.Log(temp.ToString());
            distanceArray[i] = temp;
            builds[i] = eventBuilding;
            i++;
        }
        int shortestIndex = MinValue(distanceArray);
        Debug.Log(shortestIndex.ToString() + " index");
        return builds[shortestIndex].GetComponent<Transform>();
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
        Debug.Log("floats: " + distances.Length.ToString());
        float min = 100.0f;
        int index = -1;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                index = i;
                Debug.Log("min: " + min.ToString());
            }
        }
        return index;
    }
}
