using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class CarTargetSelect : MonoBehaviour {

    private Transform hq;
    public bool isPlayerCar;
    public Building RecentBuilding { get; set; }
    public bool Working { get; set; }


    void Start ()
    {
        if (isPlayerCar)
            hq = GameObject.FindGameObjectWithTag("HQ").GetComponent<Transform>();
        else
            hq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<Transform>();
        Working = false;
        RecentBuilding = new Building();
    }


    void OnTriggerEnter(Collider other)
    {
        //ist das haus mein ziel?
        if (other.transform == GetComponent<AICharacterControl>().Target)
        {
            //braucht mich das haus noch?
            if (other.CompareTag("Haus"))
            {
                bool stay = other.GetComponent<Building>().CheckIn(this.gameObject);
                if (stay)
                {
                    Working = true;
                    RecentBuilding = GetComponent<AICharacterControl>().Target.GetComponent<Building>();
                    GetComponent<AICharacterControl>().Target = null;
                }
                else
                {
                    GetComponent<AICharacterControl>().Target = hq;
                    Working = false;
                }
            }
            else if (other.CompareTag("HQ") || other.CompareTag("AIHQ"))
            {
                GetComponent<AICharacterControl>().Target = null;
            }
        }
    }

    public void EventComplete ()
    {
        GetComponent<AICharacterControl>().Target = hq;
        Working = false;
    }
}
