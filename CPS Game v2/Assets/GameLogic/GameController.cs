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
    
    public static GameController current;
    public WaterFlowController WaterFlowController;
    public SceneLoader SceneLoader;
    public UIManager UIManager;


    public GameObject AttackerUI;

    public Reservoir Reservoir;

    public Text TurnCounter;
    //public Text ReservoirCounter;

    //public Image ScreenCover;
    public GameObject GameUI;
    public GameObject GameBoard;
    public Text TurnText;

    public int attackResource = 1;
    public int NumAvailableAttacks { get; set; }

    public int NumAvailableCheckPlacements = 0;

    public int Turn = 0;

    public int ReservoirLimit = 10;
    public int TurnLimit = 2;

    public Text TurnTimer;
    private DateTime ActiveTurnTimer;
    private DateTime StartTurnTimer;
    public int TurnDuration = 15; // Seconds
    private bool ActiveTurn;

    private bool turnisActive = false;

    public GameState GameState = GameState.AttackerTurn;

	public string gameWinner= "Attacker";

    //setups
    protected void Awake()
    {

        if(current == null)
        {
            current=this;

        }
        else
         Destroy(gameObject);

         if(NumAvailableCheckPlacements<0)
         {
             Debug.LogError("Placements must be greater than 0");
         }
    }

    //I think it doesn't have a turn timer
    protected void Start()
    {
        Debug.Log("START TURN:");
        
        StartCoroutine(TurnLoop());
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
            Turn++;            
            UIManager.current.UpdateWatcherCountTrigger();
            Debug.Log("TURN NUMBER: "+Turn);
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

            if(Turn>=TurnLimit)
            {                
                gamestart=false;
                EndGame();
            }
            yield return null;
        }

        LoadNextScene();

    }

    public void LoadNextScene()
    {
        //ADD LOADING TO NEXT SCENE HERE
    }

    private void EndGame()
    {        
        Turn=0;
		
		if( WaterFlowController.systemIsBroken() == false)
			gameWinner= "Defender";
		
        Debug.Log(gameWinner +" has won.");   

		SceneLoader.LoadNextScene();
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
            UIManager.current.AttackerTurnTrigger();

        }
        else //defender turn
        {
            UIManager.current.HideWaterIndicatorTrigger();
            UIManager.current.DefenderTurnTrigger();
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
