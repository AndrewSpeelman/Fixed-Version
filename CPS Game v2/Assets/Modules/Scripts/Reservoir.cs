using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservoir : Module
{
   


    public override void OnMouseOver()
    {
        if(GameController.current.GameState == GameState.DefenderTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UIManager.current.GuessWater(this);
                //check if there is water?
                Debug.Log("Is there water here: "+ HasFlow);        
            }

        }
    }
}
