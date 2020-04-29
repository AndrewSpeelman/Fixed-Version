using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : Module
{
    //No special functionality in pipes (yet?)
    private bool Flow;
    private bool[] Purity;

    protected override void DefenderAction()
    {

        if (DefenderVisual != null)
        {
            //check if you enough actions
            if (GameController.current.NumAvailableCheckPlacements <= 0 && FixSelected == false)
            {
                return;
            }

            //check pipe can be selected to be fixed (any pipe, broken or not)
            if (!FixSelected)
            {
                GameLogic.current.DecreaseWatchPlacement();
                FixSelected = !FixSelected;
            }
            else if (FixSelected)
            {

                GameLogic.current.IncreaseWatchPlacement();
                FixSelected = !FixSelected;
            }

            //Applies to only gameobjects below this. Intended for the scripts
            // attached to the visual particles. These can be found in ModuleVisual.cs
            gameObject.BroadcastMessage("FixedTrigger", SendMessageOptions.DontRequireReceiver);
        }
    }

    public override void Fix()
    {
        Debug.Log(FixSelected);

        //Fixes if this pipe is selected for fix (usually when confirm is pressed)
        if (FixSelected)
        {
            this.Attacked = false;
            FixSelected = !FixSelected;

            //Applies to only gameobjects below this. Intended for the scripts
            // attached to the visual particles. These can be found in ModuleVisual.cs
            gameObject.BroadcastMessage("FixedTrigger", SendMessageOptions.DontRequireReceiver);

            //Applies to only gameobjects below this. Intended for the scripts
            // attached to the visual particles. These can be found in ModuleVisual.cs
            gameObject.BroadcastMessage("AttackedTrigger", SendMessageOptions.DontRequireReceiver);


            //resimulate water
            WaterFlowController.current.SimulateWater();
        }
    }


    protected override void SetUpVisuals()
    {

        DefenderVisual = UIManager.current.DefendVisual_Pipe;
        AttackerVisual = UIManager.current.AttackVisual_Generic;
    }
}
