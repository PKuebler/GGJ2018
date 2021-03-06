﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class WinLose : MonoBehaviour {

    private bool gameEnded;

    public bool timerhasstarted;
    public int winningNr;
    public GameObject finishCanvas;
    public GameObject canvasPlayer;

    private bool endAfterTimer;
    public bool EndAfterTimer { get; set; }
    [SerializeField]
    private Stopwatch timer;
    public int GameTimer
    {
        get { return (int)timer.ElapsedMilliseconds / 1000; }
    }

    private GameObject[] buildings;
    [SerializeField]
    private int playerHouses;
    public int PlayerHouses
    { get { return playerHouses; } }
    [SerializeField]
    private int aiHouses;
    public int AIHouses
    { get { return aiHouses; } }

    // Use this for initialization
    void Start() {
        buildings = GameObject.FindGameObjectsWithTag("Haus");
        timer = new Stopwatch();
        timerhasstarted = false;
        gameEnded = false;
    }

    // Update is called once per frame
    void Update() {
        UpdateHouseCount();
    }

    private void UpdateHouseCount()
    {
        playerHouses = 0;
        aiHouses = 0;

        foreach (GameObject house in buildings)
        {
            if (house.GetComponent<Building>().owner == Building.Owner.Player)
                playerHouses++;
            if (house.GetComponent<Building>().owner == Building.Owner.AI)
                aiHouses++;
        }

        if (!gameEnded)
            CheckWhoWon();
    }

    public void StartGameTimer()
    {
        timer.Start();
        timerhasstarted = true;
    }


    private bool CheckTimer()
    {
        if (timer.ElapsedMilliseconds / 1000 > 600)
        {
            return false;
        }
        else
            return true;
    }


    private void CheckWhoWon()
    {
        if (playerHouses >= winningNr)
        {
            Won(true, false);
        }
        else if (aiHouses >= winningNr)
        {
            Won(false, false);
        }

        if (timerhasstarted && !CheckTimer())
        {
            if (playerHouses > aiHouses)
            {
                Won(true, false);
            }
            else if (playerHouses < aiHouses)
            {
                Won(false, false);
            }
            else
                Won(false, true);
        }
    }

    private void Won(bool playerWon, bool draw)
    {
        gameEnded = true;

        canvasPlayer.SetActive(false);
        finishCanvas.SetActive(true);

        Text ai = GameObject.Find ("AI Count").GetComponent<Text>();
		Text player = GameObject.Find ("Player Count").GetComponent<Text>();
		Text status = GameObject.Find ("Gewonnen").GetComponent<Text> ();

		ai.text = aiHouses.ToString () + " Clients";
		player.text = playerHouses.ToString () + " Clients";
		status.text = (draw) ? "DRAW" : ((playerWon) ? "YOU WIN" : "YOU LOSE");

        //GameObject.Find("Main Camera").SetActive(false);
        //GameObject.FindGameObjectWithTag("AIAuto").GetComponent<CarTargetSelect>().cam.SetActive(true);
    }
}
