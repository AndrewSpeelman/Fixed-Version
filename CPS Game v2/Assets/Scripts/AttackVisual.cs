using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVisual : MonoBehaviour
{
    
    ///Indiana Note: Actually does nothing/is inaccessible ???
    ///Other comments were left by previous students
    
    //this is attached to visualattackindicator. Assumes only children are
    //visual aid like particles that might use animators

    
    [SerializeField]
    private Module parentModule;

    [SerializeField]
    private Animator[] animatorList;
    // Start is called before the first frame update

    void Update()
    {
    }
    void Awake()
    {
        parentModule=GetComponentInParent<Module>();        
    }
    void Start()
    {        
        animatorList = GetComponentsInChildren<Animator>();  
    }

    private void AttackedTrigger()
    {
        Debug.Log("STATUS: "+ parentModule.Attacked);
        foreach(Animator a in animatorList)
        {
            a.SetBool("Attacked",parentModule.Attacked);
        }

    }

}
