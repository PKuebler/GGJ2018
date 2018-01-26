using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarTargetSelect : MonoBehaviour {

    AICharacterControl aicc;

	// Use this for initialization
	void Start () {
        aicc = GameObject.FindGameObjectWithTag("Auto").GetComponent<AICharacterControl>();	
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Haus")
                {
                    aicc.Target = hit.transform;
                }
                else
                {
                    Debug.Log("This isn't a House");
                }
            }
        }
    }
}
