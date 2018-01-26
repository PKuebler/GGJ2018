using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
	public bool isWaiting = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void SetWait (string name) {
		print (name);
		isWaiting = true;
	}
}
