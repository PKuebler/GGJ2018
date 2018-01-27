using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarTargetSelect : MonoBehaviour {

    private bool working;
    private bool waitingAtHQ;
    private Transform hq;
	public Transform target;

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
        if (GetComponent<AICharacterControl>().Target == null)
        {
            GetComponent<AICharacterControl>().Target = hq;
        }
        working = false;
    }
}
