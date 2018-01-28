using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler_player : MonoBehaviour {

	public GameObject hq_player;
	public WinLose winLose;
	private string moneyText;
	private Text moneyTextField;
	private string buildingsText;
	private Text buildingsTextField;

	// Use this for initialization
	void Start () {
		hq_player = GameObject.FindGameObjectWithTag ("HQ");
		winLose = GameObject.FindObjectOfType<WinLose> ().GetComponent<WinLose> ();
		moneyTextField = GameObject.Find ("Money - Text").GetComponent<Text>();
		buildingsTextField = GameObject.Find ("Buildings - Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		moneyText =  hq_player.GetComponent<HQ> ().Money.ToString() + " $";
		moneyTextField.text = moneyText;

		buildingsText =  winLose.PlayerHouses.ToString() + "/8 Clients";
		buildingsTextField.text = buildingsText;
	}
}
