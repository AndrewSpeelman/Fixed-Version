using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleVisual : MonoBehaviour
{
      [SerializeField]
    private Module parentModule;

    [SerializeField]
    private List<Animator> animatorList = new List<Animator>();
    //--note-- parentmodule doesn't get assigned and is null when a method is called. execution 
    //order is weird
    public int test = 0;
    void Update()
    {
    }
    void Awake()
    {
       
    }
    void Start()
    {        
        parentModule = GetComponentInParent<Module>();
        
        animatorList.AddRange(GetComponentsInChildren<Animator>());        
        animatorList.AddRange(GetComponents<Animator>());
        
    }
    protected void AttackedTrigger()
    {
        Debug.Log("STATUS: "+ parentModule.Attacked);
        foreach(Animator a in animatorList)
        {
            a.SetBool("Attacked",parentModule.Attacked);
        }

    }

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

    
}
