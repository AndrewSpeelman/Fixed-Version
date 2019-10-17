﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls whose turn it is, the actions available to the players, and other game logic
/// </summary>
public class GameController : MonoBehaviour
{
    
    public static GameController current;
    public WaterFlowController WaterFlowController;
    public SceneLoader SceneLoader;
    public UIManager UIManager;

    //public GameObject OraclePrefab;
    //public GameObject OracleSpawnPoint;

    public GameObject AttackerUI;

    public Reservoir Reservoir;

    public Text TurnCounter;
    //public Text ReservoirCounter;

    //public Image ScreenCover;
    public GameObject GameUI;
    public GameObject GameBoard;
    public Text TurnText;

    public int attackResource = 1;
    public int NumberOfOracles = 1;
    public int NumAvailableAttacks { get; set; }

    private int Turn = 0;

    public int ReservoirLimit = 10;
    public int TurnLimit = 15;

    public Text TurnTimer;
    private DateTime ActiveTurnTimer;
    private DateTime StartTurnTimer;
    public int TurnDuration = 15; // Seconds
    private bool ActiveTurn;

    private bool turnisActive = false;

    public GameState GameState = GameState.AttackerTurn;

    private List<Oracle> oracles;
    protected void Update()
    {
        // if (ActiveTurn) //can possibly optimize with coroutines.
        // {
        //     //Debug.Log("ACTIVE: " + GameState);
        //     ActiveTurnTimer = DateTime.Now;
        //     CheckEndTurn();
        // }
    }


    // private void CheckEndTurn()
    // {
    //     int SecondsRemaining = (TurnDuration - (ActiveTurnTimer - StartTurnTimer).Seconds);
    //     TurnTimer.text = "Time Remaining: " + SecondsRemaining.ToString();

    //     if (SecondsRemaining > 5)
    //     {
    //         TurnTimer.color = new Color(.79f, .82f, .16f);
    //     }
    //     else if (SecondsRemaining % 2 == 0)
    //     {
    //         TurnTimer.color = new Color(1f, .3f, .15f);
    //     }
    //     else
    //     {
    //         TurnTimer.color = new Color(1f, .2f, 0);
    //     }

    //     if (ActiveTurnTimer > StartTurnTimer.AddSeconds(TurnDuration))
    //     {
    //         EndTurn();
    //     }
    // }

    //setups
    protected void Awake()
    {

        if(current == null)
        {
            current=this;

        }
        else
         Destroy(gameObject);
    }

    //I think it doesn't have a turn timer
    protected void Start()
    {
        //added here so that capacity=1 in module doesn't override. Could also change script execution order
        Reservoir.Capacity = ReservoirLimit;

        //makes oracles-- owls---
        /* 
        for (int i = 0; i < this.NumberOfOracles; i++)
        {
            var newOracle = Instantiate(this.OraclePrefab, new Vector3(this.OracleSpawnPoint.transform.position.x + (i * 2),
                this.OracleSpawnPoint.transform.position.y, this.OracleSpawnPoint.transform.position.z),
                this.OraclePrefab.transform.rotation);
            oracles.Add(newOracle.GetComponent<Oracle>());
        }*/

        Debug.Log("START TURN:");
        
        StartCoroutine(TurnLoop());
        Debug.Log("Game has finished");
        StartTurnTimer = DateTime.Now;
        ActiveTurn = true;
    }

    
    

    IEnumerator  TurnLoop()
    {
        //--setupgame
        //put pre game start here
        WaterFlowController.SimulateWater();
        bool gamestart= true;
        ActiveTurn=false;

        while(gamestart)
        {
            Debug.Log(GameState + " is starting");
            StartTurn();            
            Debug.Log(GameState + " setup is complete");
            //update ui to new turn
            UIManager.SetUpTurn(GameState);
            //Debug.LogError("STOPHERE");
            while(ActiveTurn) //loop until turn is done.
            {
                yield return null; //code will resume after next update
                //Debug.LogError("LOOPING");
            }
            EndTurn();
            Debug.Log(GameState + " has ended.");
            yield return null;
        }

    }

    public void NextTurn()
    {
        ActiveTurn = false;
    }
    public void StartTurn()
    {
        if(this.GameState == GameState.AttackerTurn)
        {
            UIManager.current.ShowWaterIndicatorTrigger();

        }
        else //defender turn
        {
            UIManager.current.HideWaterIndicatorTrigger();
        }
        ActiveTurn=true;
        


    }
    public void EndTurn()
    {
        if(GameState == GameState.AttackerTurn)
        {
            //attacker ending turn
            GameState = GameState.DefenderTurn;

        }
        else 
        {
            //defender ending turn
            GameState = GameState.AttackerTurn; 
        }

    }

}
