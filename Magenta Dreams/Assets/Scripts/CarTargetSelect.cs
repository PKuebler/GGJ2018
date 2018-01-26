using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarTargetSelect : MonoBehaviour {



	public void ReachedTarget(GameObject triggerObj, bool isWorking)
    {
        if (isWorking)
        {
            GetComponent<AICharacterControl>().Target = null;
        }
    }

    public void EventFinished()
    {

    }
}
