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
    public AttackerAI AttackerAI;
    //calling all the other controller files in gamelogic directory

    public GameObject AttackerUI;

    public Reservoir Reservoir;

    public Text TurnCounter;
    //public Text ReservoirCounter;

    //public Image ScreenCover;
    public GameObject GameUI;
    public GameObject GameBoard;
    public Text TurnText;

    public int attackResource = 1;

    public int NumAvailableCheckPlacements = 0;
    public int AttacksAvailable = 0;

    public int Turn = 0; //turn timer starts at 0
    public int DefenderTurns = 0;

    public int ReservoirLimit = 10;
    public int TurnLimit; //value is set in unity

    public Text TurnTimer;
    private DateTime ActiveTurnTimer;
    private DateTime StartTurnTimer;
    public int TurnDuration = 15; // Seconds/dont think this is actually used
    private bool ActiveTurn;

    private bool turnisActive = false;

    public GameState GameState = GameState.AttackerTurn;

	public string gameWinner= "Attacker";//is default gamewinner attacker..?

    //setups
    protected void Awake()
    {//i think this just starts the game? not sure.

        if (current == null)
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
            if (this.GameState == GameState.DefenderTurn)
            {
                DefenderTurns++;
            }
            UIManager.current.UpdateWatcherCountTrigger(); //fills the number of watchers the defender has..? shouldnt this be on defender turn only?
            UIManager.current.UpdateAttackCountTrigger();
            Debug.Log("TURN NUMBER: "+Turn);
            Debug.Log(GameState + " is starting");
            StartTurn();            
            Debug.Log(GameState + " setup is complete");
            //update ui to new turn
            UIManager.SetUpTurn(GameState);
            //Debug.LogError("STOPHERE");
            CallAI();
            while(ActiveTurn) //loop until turn is done.
            {
                
                yield return null; //code will resume after next update
                //Debug.LogError("LOOPING");
            }
            EndTurn();
            Debug.Log("turn number = "+ Turn + " of" +TurnLimit);
            Debug.Log(GameState + " has ended.");

            if(DefenderTurns>=TurnLimit)
            {                 //ends round when turn limit is up
                gamestart =false;
                EndGame();
            }
            yield return null;
        }

        LoadNextScene();

    }

    public void LoadNextScene()
    { //look into sceneloader
      //ADD LOADING TO NEXT SCENE HERE
        if (gameWinner == "Defender")
			SceneLoader.LoadDefenderVictory();
		else
			SceneLoader.LoadAttackerVictory();
    }

    private void EndGame()
    {         //heres the code for the end of game screen. shouldnt be hard to create a function that goes back to the main menu i think.
        //should be able to just call sceneloader.startscreen or something when a button is pressed.
        Turn = 0;
        DefenderTurns = 0;
		
		if( WaterFlowController.systemIsBroken() == false)
			gameWinner= "Defender";
		else
			gameWinner= "Attacker";
		

    }

    public void NextTurn()
    {
        ActiveTurn = false;
    }
    public void StartTurn()
    { //this is all setup, dont put the attacker ai attack function call here.
        UIManager.current.ToggleAttackerUI();
        UIManager.current.ToggleDefenderUI();
        if (this.GameState == GameState.AttackerTurn)  //its the attacker turn do this.
        {
     //       UIManager.current.ShowWaterIndicatorTrigger(); //calling ui manager
    //        UIManager.current.AttackerTurnTrigger();
     
          

        }
        else //defender turn
        {
            //Confirms placement of watchers.
            for (int i = 0; i < GameBoard.GetComponentsInChildren<Reservoir>().Length; i++)
            {
                //Reset the placement of watchers. 
                GameBoard.GetComponentsInChildren<Reservoir>()[i].ResetPlaced();
            }
            UIManager.current.HideWaterIndicatorTrigger();
            UIManager.current.DefenderTurnTrigger();
            
        }
        ActiveTurn=true;   
    }

    public void CallAI()
    {
      if (this.GameState == GameState.AttackerTurn)  
      {
             AttackerAI.Attack();
             NextTurn();
      }
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
