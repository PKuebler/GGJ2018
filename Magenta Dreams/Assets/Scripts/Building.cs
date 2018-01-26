using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
	public bool isWaiting = false;
	public int player = -1; // if -1 -> no client

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void SetAction () {
		print (gameObject.name);

		if (isWaiting != true) {
			GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylinder.name = "Icon Waiting!";
			cylinder.transform.parent = gameObject.transform;
			cylinder.transform.position = gameObject.transform.position - new Vector3(0, - (gameObject.transform.localScale.y * 2), 0);
			cylinder.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
		isWaiting = true;
	}

    void OnTriggerEnter(Collider other)
    {
        //Auto: Fahrt stoppen wenn zielobjekt erreicht
        //dort wird verglichen, ob es der Trigger vom Ziel des Autos war
        //- wenn ja: Stoppen
        //- wenn nein: weiterfahren
        if (other.CompareTag("Auto"))
        {
            other.gameObject.GetComponent<CarTargetSelect>().ReachedTarget(this.gameObject);
        }
    }
}
