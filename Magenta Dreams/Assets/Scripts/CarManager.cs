using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CarManager : MonoBehaviour {

    [SerializeField]
    private GameObject selectedObject;
    private HQ playerHQ;
	private Timer Timer;
    public AudioClip driveClip;

    // Use this for initialization
    void Start()
    {
        playerHQ = GameObject.FindGameObjectWithTag("HQ").GetComponent<HQ>();
		Timer = gameObject.GetComponent<Timer> ();
    }


    // Update is called once per frame
    void Update()
    {
        SelectCarsByNumbers();
        
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
                        selectedObject.GetComponent<AudioSource>().PlayOneShot(driveClip);
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


    private void SelectCarsByNumbers()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (Input.GetKeyUp(KeyCode.Return))
		{
			Timer.StartGame ();
		}
		if (Input.GetButtonUp("1"))
		{
			if (playerHQ.carList.Count < 1)
				return;
			selectedObject = playerHQ.carList [0];
		}
		if (Input.GetButtonUp("2"))
		{
			if (playerHQ.carList.Count < 2)
				return;
			selectedObject = playerHQ.carList [1];
		}
		if (Input.GetButtonUp("3"))
		{
			if (playerHQ.carList.Count < 3)
				return;
			selectedObject = playerHQ.carList [2];
		}
		if (Input.GetButtonUp("4"))
		{
			if (playerHQ.carList.Count < 4)
				return;
			selectedObject = playerHQ.carList [3];
		}
		if (Input.GetButtonUp("5"))
		{
			if (playerHQ.carList.Count < 5)
				return;
			selectedObject = playerHQ.carList [4];
		}
		if (Input.GetButtonUp("6"))
		{
			if (playerHQ.carList.Count < 6)
				return;
			selectedObject = playerHQ.carList [5];
		}
		if (Input.GetButtonUp("7"))
		{
			if (playerHQ.carList.Count < 7)
				return;
			selectedObject = playerHQ.carList [6];
		}
		if (Input.GetButtonUp("8"))
		{
			if (playerHQ.carList.Count < 8)
				return;
			selectedObject = playerHQ.carList [7];
		}
		if (Input.GetButtonUp("9"))
		{
			if (playerHQ.carList.Count < 9)
				return;
			selectedObject = playerHQ.carList [8];
		}

    }
}
