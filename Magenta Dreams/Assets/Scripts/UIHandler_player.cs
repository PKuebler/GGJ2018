using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler_player : MonoBehaviour {

	public GameObject hq_player;
	private string moneyText;
	private Text moneyTextField;
	private string carText;
	private Text carTextField;

	// Use this for initialization
	void Start () {
		hq_player = GameObject.FindGameObjectWithTag ("HQ");
		moneyTextField = GameObject.Find ("Money - Text").GetComponent<Text>();
		carTextField = GameObject.Find ("Auto Anzahl Test").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		moneyText =  hq_player.GetComponent<HQ> ().money.ToString();
		moneyTextField.text = moneyText;

		carText = hq_player.GetComponent<HQ> ().cars.ToString ();
		carTextField.text = carText;
	}
}
