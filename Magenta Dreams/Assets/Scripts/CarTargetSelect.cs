using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarTargetSelect : MonoBehaviour {

    private bool working;
    private bool waitingAtHQ;
    private Transform hq;

    void Start ()
    {
        working = false;
        waitingAtHQ = false;
        hq = GameObject.FindGameObjectWithTag("HQ").GetComponent<Transform>();
    }

	public void ReachedTarget(GameObject triggerObj, bool isWorking)
    {
        if (isWorking && !working)
        {
            GetComponent<AICharacterControl>().Target = null;
            working = true;
        }
        else if (!isWorking)
        {
            GetComponent<AICharacterControl>().Target = hq;
        }
    }

    public void EventFinished()
    {
        working = false;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("HQ") && !waitingAtHQ)
        {
            GetComponent<AICharacterControl>().Target = null;
            waitingAtHQ = true;
        }
    }

    void OnTriggerLeave (Collider other)
    {
        if (other.gameObject.CompareTag("HQ"))
        {
            waitingAtHQ = false;
        }
    }
}
