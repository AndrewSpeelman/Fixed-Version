using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reservoir : Module
{
   
   private ModuleVisual DefenderVisual_child;
   private bool checkPlaced;
   private bool checkPlacedConfirmed;
    
    protected override void SetUpVariables()
    {
        canbeAttacked = false;
        checkPlaced = false;
        checkPlacedConfirmed = false;

        
    }

    protected override void AfterSetup()
    {
        DefenderVisual_child = DefenderVisual.GetComponent(typeof(ModuleVisual)) as ModuleVisual;

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

    
    /*
        ---What to do visually when defender clicks on this module--

        Checks if water is here. Has to accept on a seperate ui button to confirm.
        //Add to list of areas to check
        //Update visual that this module has added to look out for
     */
    protected override void DefenderAction()
    {
        //make sure there are visuals and that it is not already confirmed as a watcher location.
        if (DefenderVisual != null && checkPlacedConfirmed == false)
        {
            //check if you enough watchers to place
            if (GameController.current.NumAvailableCheckPlacements <= 0 && checkPlaced == false)
            {
                return;
            }

            //check if check can be placed
            if (!checkPlaced)
            {
                GameLogic.current.DecreaseWatchPlacement();
                DefenderVisual_child.SetCheckPlacement(true);
                checkPlaced = !checkPlaced;
            }
            else if (checkPlaced)
            {

                GameLogic.current.IncreaseWatchPlacement();
                DefenderVisual_child.SetCheckPlacement(false);
                checkPlaced = !checkPlaced;
            }
        }
    }

    public void ConfirmPlaced()
    {
        //used to keep track of which reservoirs are confirmed
        if (checkPlaced)
        {
            checkPlaced = false;
            checkPlacedConfirmed = true;
        }
    }

    //Used to reset watchers at the end of the turn.
    public void ResetPlaced()
    {
        if(checkPlacedConfirmed)
        {
            //mark as unconfirmed
            checkPlacedConfirmed = false;

            //disable visuals
            DefenderVisual_child.SetCheckPlacement(false);
            DefenderVisual_child.RemoveIndicator();


        }
    }

    #region HandleIndicators
    private void onShowWaterIndicator()
    {
        if (HasFlow)
            WaterIndicator.SetActive(true);
    }


    #endregion 

    protected override void SetUpVisuals()
    {
        
         DefenderVisual= UIManager.current.DefendVisual_Reservoir;
         AttackerVisual= UIManager.current.AttackVisual_Reservoir;
    }
    #endregion
}
