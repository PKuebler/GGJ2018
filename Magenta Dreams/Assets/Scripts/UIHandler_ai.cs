using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler_ai : MonoBehaviour {

	public GameObject hq_ai;
	private string moneyText;
	private Text moneyTextField;
	private string carText;
	private Text carTextField;

	// Use this for initialization
	void Start () {
		hq_ai = GameObject.FindGameObjectWithTag ("AIHQ");
		moneyTextField = GameObject.Find ("MoneyAI - Text").GetComponent<Text>();
		carTextField = GameObject.Find ("AutoAI Anzahl Test").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		moneyText =  hq_ai.GetComponent<HQ> ().Money.ToString();
		moneyTextField.text = moneyText;

		carText = hq_ai.GetComponent<AIManager> ().CarList.Count.ToString ();
		carTextField.text = carText;
	}
}
