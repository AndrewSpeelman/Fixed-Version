using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleVisual : MonoBehaviour
{
    //------------IMPORTANT!----------
    //---- In the future let the string variables be static instead of setting it in code---
    //------------IMPORTANT!----------

    [SerializeField]
    private Module parentModule;

    [SerializeField]
    private List<Animator> animatorList = new List<Animator>();
    //--note-- parentmodule doesn't get assigned and is null when a method is called. execution 
    //order is weird
    
    void Update()
    {
    }
    void Awake()
    {
       
    }
    void Start()
    {        
        
        UIManager.current.onConfirmCheckPlacementTrigger += ConfirmTrigger;

        parentModule = GetComponentInParent<Module>();
        
        animatorList.AddRange(GetComponentsInChildren<Animator>());        
        animatorList.AddRange(GetComponents<Animator>());
        
    }

    
    private void OnDestroy()
    {
        UIManager.current.onConfirmCheckPlacementTrigger -= ConfirmTrigger;
    }

    
    
    //Confirm that this module is being watched
    //assumes that all 
    private void ConfirmTrigger()
    {
        if(GameController.current.NumAvailableCheckPlacements<0)
        {
            Debug.LogError("Placements have been exceeded");
            
        }
        foreach(Animator a in animatorList)
        {
            foreach(AnimatorControllerParameter b in a.parameters)
            {
                if(b.name=="confirm")
                {
                    a.SetTrigger("confirm");
                    GameLogic.current.WatchThisNode(parentModule);
                    continue;
                }
            }
        }

    }

    /*
    private void Reset()
    {
        
        foreach(Animator a in animatorList)
        {
           //reset here
        }
        //maybe do it for every turn change or where it is forced upon the user or the user themselves

    }*/

    #region BroadcastMessages
    protected void AttackedTrigger()
    {
        Debug.Log("attacked");
        foreach (Animator a in animatorList)
        {
            a.SetBool("Attacked",parentModule.Attacked);
        }
    }

    //Changes the particle for defender fixing pipes
    protected void FixedTrigger()
    {
        Debug.Log("fixed");
        foreach (Animator a in animatorList)
        {
           a.SetBool("FixSelected", parentModule.FixSelected);
        }
    }
    //set the ui conditions to set the water to show true/false
    protected void SetWater(bool isActive)
    {
        foreach(Animator a in animatorList)
        {

            a.SetBool("hasWater",isActive);
        }
        
    }
    //
    protected void SwitchingTurn(string id)
    {
        //weird script execution problem. fix more later
        if(parentModule == null)
        {
            parentModule = GetComponentInParent<Module>();
        }

        //Debug.Log("parentModule: "+parentModule.name);
        //Debug.Log("NAME: "+gameObject.name +"---ID: "+id);
        //bool isEqual= ;
        //Debug.Log("---"+isEqual+"---");
        if(gameObject.name == id)
        {
            GameObject child= gameObject.transform.GetChild(0).gameObject;
            child.SetActive(true);
        }
        else
        {
            GameObject child= gameObject.transform.GetChild(0).gameObject;
            child.SetActive(false);
        }
    }

   

    #endregion
    // Refactor this out into defender Module Visual Child script
    // at some point
    public void SetCheckPlacement(bool shouldPlace)
    {
        Debug.Log("Placed check on " + parentModule  + ". Click button confirm");
        foreach(Animator a in animatorList)
        {
            a.SetBool("checkhasbeenPlaced",shouldPlace);
            a.SetBool("Reset", false);
            a.SetBool("Remove", false);
        }
    }

    public void RemoveIndicator()
    {
        foreach (Animator a in animatorList)
        {
            a.SetTrigger("Remove");
        }

        foreach (Animator a in animatorList)
        {
            a.SetTrigger("Reset");
        }
        Debug.Log("resetting watchers");
    }



}
