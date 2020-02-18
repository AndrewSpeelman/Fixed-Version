using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //This class is strictly for the gamelogic of this scene.
    //Deals with logic pertaining to the game
    // Gamecontroller should deal with setting up turns/macro decisions
    // Gamelogic is more micro
    public static GameLogic current;
    public List<Module> modulesbeingWatched= new List<Module>();

    // Start is called before the first frame update
    void Awake()
    {
        if(current == null)
        {
            current=this;

        }
        else
         Destroy(gameObject);    
    }

    //check https://youtu.be/gx0Lt4tCDE0
    

    //activates the UI signal and adjust modules accordingly
    //Checking placement is only a visual clue so the module itself does not change
    // but only the visual clues. The only logic that changes is if the defender can place more
    // or not.
    public void ConfirmCheckPlacementTrigger()
    {
        
        //UI
        UIManager.current.ConfirmWaterTrigger();
        //Non UI Logic

        //Activate all pipe fixes when confirm is selected.
        for (int i = 0; i < GameController.current.GameBoard.GetComponentsInChildren<Pipe>().Length; i++)
        {
            //Fix only fixes pipes if they are selected for fixing. 
            GameController.current.GameBoard.GetComponentsInChildren<Pipe>()[i].Fix();
        }

        //Confirms placement of watchers.
        for (int i = 0; i < GameController.current.GameBoard.GetComponentsInChildren<Reservoir>().Length; i++)
        {
            //Reset the placement of watchers. 
            GameController.current.GameBoard.GetComponentsInChildren<Reservoir>()[i].ConfirmPlaced();
        }
    }   

    public void WatchThisNode(Module m)
    {
        modulesbeingWatched.Add(m);

    }

    //decreases the number of available watchers and updates UI
    public void DecreaseWatchPlacement()
    {
        GameController.current.NumAvailableCheckPlacements--;
        UIManager.current.UpdateWatcherCountTrigger();


    }

    //increases the number of available watchers and updates UI
    public void IncreaseWatchPlacement()
    {
        GameController.current.NumAvailableCheckPlacements++;
        UIManager.current.UpdateWatcherCountTrigger();

        
        //check if it goes overboard to catch a bug
        
    }

    //decreases the number of available attacks and updates UI
    public void DecreaseAttackCount()
    {
        GameController.current.AttacksAvailable--;
        UIManager.current.UpdateAttackCountTrigger();


    }

    //increses the number of available attacks and updates UI
    public void IncreaseAttackCount()
    {
        GameController.current.AttacksAvailable++;
        UIManager.current.UpdateAttackCountTrigger();


        //check if it goes overboard to catch a bug

    }
}
