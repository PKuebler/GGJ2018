using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class CarTargetSelect : MonoBehaviour {

    private Transform hq;
    public ParticleSystem part;
    public bool isPlayerCar;

    public bool Working { get; set; }


    void Start ()
    {
        if (isPlayerCar)
            hq = GameObject.FindGameObjectWithTag("HQ").GetComponent<Transform>();
        else
            hq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<Transform>();
        Working = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //ist das haus mein ziel?
        if (other.transform == GetComponent<AICharacterControl>().Target)
        {
            //braucht mich das haus noch?
            bool stay = other.GetComponent<Building>().CheckIn(this.gameObject);
            if (stay)
            {
                Working = true;
                GetComponent<AICharacterControl>().Target = null;
            }
            else
            {
                GetComponent<AICharacterControl>().Target = hq;
                Working = false;
            }
        }
    }

    public void EventComplete ()
    {
        GetComponent<AICharacterControl>().Target = hq;
        Working = false;
    }
}
