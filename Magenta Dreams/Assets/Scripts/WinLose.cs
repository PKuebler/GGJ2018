using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WinLose : MonoBehaviour {

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

        CheckWhoWon();
    }

    public void StartGameTimer()
    {
        timer.Start();
    }


    private void CheckTimer()
    {
        if (timer.ElapsedMilliseconds / 1000 > 180)
        {
            CheckWhoWon();
        }
    }


    private void CheckWhoWon()
    {

    }
}
