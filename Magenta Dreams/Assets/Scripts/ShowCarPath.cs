using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShowCarPath : MonoBehaviour {
	private NavMeshAgent agent;
	private GameObject lineObj;
	private LineRenderer line;

	// Use this for initialization
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent> ();

		// line
		lineObj = new GameObject();
		lineObj.name = "Path";
		lineObj.transform.parent = gameObject.transform;
		line = lineObj.AddComponent<LineRenderer>();
		line.startWidth = 0.2f;
		line.endWidth = 0.2f;
		line.material = gameObject.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.path.corners.Length < 2) {
			lineObj.SetActive (false);
			return;
		} else if (!lineObj.activeSelf) {
			lineObj.SetActive(true);
		}

		line.positionCount = agent.path.corners.Length;

		for (int i = 0; i < agent.path.corners.Length; i++) {
			line.SetPosition (i, agent.path.corners [i]);
		}
	}
}
