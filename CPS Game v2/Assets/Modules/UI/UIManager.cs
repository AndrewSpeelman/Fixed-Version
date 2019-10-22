using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    
    public event Action onHideWaterIndicatorTrigger;
    public event Action onShowWaterIndicatorTrigger;
    public event Action onAttackerTurnTrigger;
    public event Action onDefenderTurnTrigger;
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
    
    public void GuessWater(Module m)
    {
        m.DefenderVisual.SetActive(!m.DefenderVisual.activeSelf);
    }

    


}
