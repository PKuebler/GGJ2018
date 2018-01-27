using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public class CarTargetSelect : MonoBehaviour {

    private Transform hq;
	public Transform target;
    public ParticleSystem part;
    public bool isPlayerCar;

    public NavMeshAgent agent;

    public bool Working { get; private set; }


    void Start ()
    {
        if (isPlayerCar)
            hq = GameObject.FindGameObjectWithTag("HQ").GetComponent<Transform>();
        else
            hq = GameObject.FindGameObjectWithTag("AIHQ").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        Working = false;
    }

	public void ReachedTarget(GameObject triggerObj, bool isWorking)
    {
        if (isWorking)
        {
            GetComponent<AICharacterControl>().Target = null;
            Working = true;
        }
        else if (!isWorking)
        {
            Debug.Log("reached target - false");
            GetComponent<AICharacterControl>().Target = hq;
            Working = false;
        }
    }

    public void EventFinished()
    {

    }
}
