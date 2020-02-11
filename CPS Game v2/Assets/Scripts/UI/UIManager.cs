using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   /*
        All Methods in here do not affect gamelogic in anyway.
        Only UI elements are being managed.         
     */
    public static UIManager current;
    public Text TurnText;
    [SerializeField]
    public  GameObject AttackVisual_Generic;
    [SerializeField]
    public  GameObject DefendVisual_Generic;
    [SerializeField]
    public  GameObject AttackVisual_Resovoir;
    [SerializeField]
    public  GameObject DefendVisual_Resovoir;
    
    //The team names. Could update with player names?
    [SerializeField]
    private string AttackerName = "Attacker";
    [SerializeField]
    private string DefenderName = "Defender";

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
    //Various game events, just catches and calls a function on event trigger (see video above)
    public event Action onHideWaterIndicatorTrigger;
    public event Action onShowWaterIndicatorTrigger;
    public event Action onAttackerTurnTrigger;
    public event Action onDefenderTurnTrigger;    
    public event Action onCheckIfThereIsWaterTrigger;
    public event Action onConfirmCheckPlacementTrigger;    
    public event Action onUpdateWatcherCountTrigger;
    public event Action onUpdateTurnCountTrigger;
    
    public void HideWaterIndicatorTrigger()
    {
        if(onHideWaterIndicatorTrigger!=null)
        {
            onHideWaterIndicatorTrigger();
        }
    }    
    public void ShowWaterIndicatorTrigger()
    {
        if(onShowWaterIndicatorTrigger!=null)
        {
            onShowWaterIndicatorTrigger();
        }
    }
    public void DefenderTurnTrigger()
    {
        if(onDefenderTurnTrigger!=null)
        {
            onDefenderTurnTrigger();
        }
    }
    public void AttackerTurnTrigger()
    {
        if(onAttackerTurnTrigger!=null)
        {
            onAttackerTurnTrigger();
        }
    }


    public void UpdateWatcherCountTrigger()
    {
        if(onUpdateWatcherCountTrigger!=null)
        {
            onUpdateWatcherCountTrigger();
        }
    }    

    public void UpdateTurnCountTrigger()
    {
        if(onUpdateTurnCountTrigger!=null)
        {
            onUpdateTurnCountTrigger();
        }
    }    
    //Assumes 
    public void CheckIfThereIsWaterTrigger()
    {
        if(onCheckIfThereIsWaterTrigger!=null)
        {
            onCheckIfThereIsWaterTrigger();
        }
    }
    //----------------
    public void SetUpTurn(GameState currentState)
    {
        UpdateTurnText(currentState);
        if(currentState == GameState.DefenderTurn)
        {
                                   
        }
        else 
        {
        }

    }

    //Updates the name display. Names updated to are above in this file.
    public void UpdateTurnText(GameState currentState)
    {
        if(currentState == GameState.DefenderTurn)
        {
            TurnText.text = DefenderName;
        }
        else 
        {
            TurnText.text = AttackerName;
        }

    }

    //Defender Visual Actions
    
    public void ConfirmWaterTrigger()
    {
        if(onConfirmCheckPlacementTrigger!=null)
        {
            onConfirmCheckPlacementTrigger();
        }
    }

    


}
