using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls whose turn it is, the actions available to the players, and other game logic
/// </summary>
public class GameController : MonoBehaviour
{
    public WaterFlowController WaterFlowController;
    public SceneLoader SceneLoader;

    public GameObject OraclePrefab;
    public GameObject OracleSpawnPoint;

    public GameObject AttackerUI;

    public Reservoir Reservoir;

    public Text TurnCounter;
    public Text ReservoirCounter;

    public Image ScreenCover;
    public GameObject GameUI;
    public GameObject GameBoard;
    public Text TurnText;

    public int NumberOfAttacksPerTurn = 1;
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
        if (ActiveTurn) //can possibly optimize with coroutines.
        {
            //Debug.Log("ACTIVE: " + GameState);
            ActiveTurnTimer = DateTime.Now;
            CheckEndTurn();
        }
    }


    private void CheckEndTurn()
    {
        int SecondsRemaining = (TurnDuration - (ActiveTurnTimer - StartTurnTimer).Seconds);
        TurnTimer.text = "Time Remaining: " + SecondsRemaining.ToString();

        if (SecondsRemaining > 5)
        {
            TurnTimer.color = new Color(.79f, .82f, .16f);
        }
        else if (SecondsRemaining % 2 == 0)
        {
            TurnTimer.color = new Color(1f, .3f, .15f);
        }
        else
        {
            TurnTimer.color = new Color(1f, .2f, 0);
        }

        if (ActiveTurnTimer > StartTurnTimer.AddSeconds(TurnDuration))
        {
            EndTurn();
        }
    }

    //setups
    protected void Awake()
    {
        this.NumAvailableAttacks = this.NumberOfAttacksPerTurn;

        Results.ReservoirLimit = ReservoirLimit;// not sure about this maybe should put properly into a script in a serperate gameobject

        oracles = new List<Oracle>();
        TurnText.gameObject.SetActive(true);
        ScreenCover.gameObject.SetActive(false);
        ScreenCover.fillCenter = true;

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
        this.EndTurn();
        Debug.Log("END TURN:");
        StartTurnTimer = DateTime.Now;
        ActiveTurn = true;
    }

    public void EndTurn()
    {
        ActiveTurn = false;

        if (this.GameState == GameState.AttackerTurn) //defender
        {

            this.GameState = GameState.DefenderTurn;
            Debug.Log(GameState);
            this.AttackerUI.SetActive(false);
            TurnText.text = "Defender's Turn";
            TurnText.color = new Color(0, .5F, 1F);
        }
        else //attacker turn
        {

            this.GameState = GameState.AttackerTurn;

            Debug.Log("THIS IS:" + GameState);
            this.NumAvailableAttacks = this.NumberOfAttacksPerTurn; //resets

            this.AttackerUI.SetActive(true);
            
            WaterFlowController.SimulateWater();

            /* 
            for (int i = 0; i < 2; i++)
            {
                this.WaterFlowController.TickModules();

            }*/

            //OracleEnabler();

            if (++Turn >= TurnLimit)
            {
                Results.ReservoirFill = Reservoir.WaterList.Count;

                Debug.Log("LoadingNextScene");
                this.SceneLoader.LoadNextScene();
            }
            ReservoirCounter.text = Reservoir.WaterList.Count.ToString();
            TurnCounter.text = "Turn: " + Turn + "/" + TurnLimit;
            TurnText.text = "Attacker's Turn";
            TurnText.color = new Color(1F, 0, 0);

        }

        ScreenCover.gameObject.GetComponentsInChildren<Text>()[0].text = TurnText.text;
        ScreenCover.gameObject.GetComponentsInChildren<Text>()[0].color = TurnText.color;

        StartCoroutine(WaitForClick());
    }




    protected IEnumerator WaitForClick()
    {
        //Puts up the screen cover
        ScreenCover.gameObject.SetActive(true);
        GameUI.SetActive(false);
        GameBoard.SetActive(false);
        TurnTimer.gameObject.SetActive(false);

        //waits for click
        yield return new WaitWhile(() => !Input.GetMouseButtonDown(0));

        //puts doesn screen cover
        ScreenCover.gameObject.SetActive(false);
        TurnTimer.gameObject.SetActive(true);
        GameUI.SetActive(true);
        GameBoard.SetActive(true);

        ActiveTurn = true;
        StartTurnTimer = DateTime.Now;
        //OracleEnabler();
    }
    /* 
    private void OracleEnabler()
    {

        if (this.GameState == GameState.DefenderTurn)
        {
            Debug.Log("Enabling Oracles");
            foreach (Oracle o in this.oracles) //enable oracles
            {
                o.InputActive = true;
                o.setAnimationState("searching"); //possibly change this to a global like gamestate
            }
        }
        else
        {
            Debug.Log("Disabling Oracles");
            foreach (Oracle o in this.oracles) //disable oracles
            {
                o.InputActive = false;
                o.ApplyRule();
                o.setAnimationState("idle"); //possibly change this to a global like gamestate
            }

        }
    }*/
}
