using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackerAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         
    }
    
    public WaterFlowController WaterFlowController;
    //add priority list
    public List<int> priority = new List<int>();
    
    public void Attack()
    {
      for (int i=0; i<priority.Count; i++)
      {
        WaterFlowController.ModuleList[priority[i]].Attack();
        
      }
    
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
