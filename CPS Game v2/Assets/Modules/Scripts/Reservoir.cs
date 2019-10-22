using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservoir : Module
{
   

    protected override void SetUpVariables()
    {
        canbeAttacked = false;
    }

    public override void OnMouseOver()
    {
        if(GameController.current.GameState == GameState.DefenderTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //UIManager.current.GuessWater(this);
                
                gameObject.BroadcastMessage("AttackedTrigger");
                //check if there is water?
                Debug.Log("Is there water here: "+ HasFlow);        
            }

        }
    }

    //Overriedes
    #region OverrideArea
    protected override void  HandleAttackVisualIndicator()
    {
        //AttackerIndicators    
        AttackerVisual = new GameObject();

        AttackerVisual.name = "Attacker Visual";
        AttackerVisual.transform.SetParent(this.gameObject.transform);
        AttackerVisual.transform.position = transform.position + visualOffset;

    }
    
    protected override void SetUpVisuals()
    {
        
         DefenderVisual= UIManager.current.DefendVisual_Resovoir;
         AttackerVisual= UIManager.current.AttackVisual_Resovoir;
    }
    #endregion
}
