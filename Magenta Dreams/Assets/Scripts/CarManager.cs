using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarManager : MonoBehaviour {

    [SerializeField]
    private GameObject selectedObject;
    private HQ playerHQ;

    // Use this for initialization
    void Start()
    {
        playerHQ = GameObject.FindGameObjectWithTag("HQ").GetComponent<HQ>();
    }


    // Update is called once per frame
    void Update()
    {
        //SelectCarsByNumbers();
        
        //1. Nicht selektiert: wenn Auto: selected = Auto
        //2. Auto selektiert: a) wenn Haus: Ziel / b) wenn neues Auto: neues Auto = selected / c) wenn nichts: selected = nichts
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //1. Fall: Nichts selektiert
                if (selectedObject == null)
                {
                    if (hit.transform.tag == "Auto")
                    {
                        selectedObject = hit.transform.gameObject;
                    }
                }
                //2. Fall: Auto selektiert
                //2a) Haus selektiert
                else if (hit.transform.tag == "Haus")
                {
                    //ist das haus schon das ziel?
                    if (hit.transform != selectedObject.GetComponent<AICharacterControl>().Target)
                    {
                        //arbeite ich gerade?
                        if (selectedObject.GetComponent<CarTargetSelect>().Working)
                        {
                            selectedObject.GetComponent<CarTargetSelect>().RecentBuilding.CheckOut(selectedObject.gameObject);
                        }
                        selectedObject.GetComponent<AICharacterControl>().Target = hit.transform;
                        selectedObject.GetComponent<AICharacterControl>().ReachedTarget = false;
                        selectedObject.GetComponent<CarTargetSelect>().Working = false;
                    }
                }
                //2b) Neues Auto selektiert
                else if (hit.transform.tag == "Auto")
                {
                    selectedObject = hit.transform.gameObject;
                }
                //2c) Klick in die Landschaft: Auto wird abgewählt
                else
                {
                    selectedObject = null;
                }
            }
        }
    }

    private void OnGUI()
    {
        if (selectedObject == null)
            GUI.Label(new Rect(0, 0, 100, 50), "Aktives Objekt: Kein Objekt gewählt.");
        else
        {
            GUI.Label(new Rect(0, 0, 100, 50), "Aktives Objekt: " + selectedObject.gameObject.name);
            if (selectedObject.GetComponent<AICharacterControl>().Target != null)
            {
                GUI.Label(new Rect(0, 50, 100, 50), "Ziel des Autos: " + selectedObject.GetComponent<AICharacterControl>().Target.name);
            }
            else
            {
                GUI.Label(new Rect(0, 50, 100, 50), "Ziel des Autos: Kein Ziel gewählt");
            }
        }
    }


    private void SelectCarsByNumbers()
    {
        if (playerHQ.carList.Count >= 1)
        {
            if (Input.GetButtonUp("1"))
            {

            }
        }

    }
}
