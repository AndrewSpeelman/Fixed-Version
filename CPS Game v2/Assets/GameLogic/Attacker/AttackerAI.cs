using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackerAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         
    }
    public GameController GameController;
    public WaterFlowController WaterFlowController;
    //add priority list
    public List<int> priority = new List<int>(); //will probably want to make a function
      //to randomize/autoate piority list but hardcode for now
    int attackcount = 0; //testing counter to make sure its not attacking more than 2 total times
    public void Attack()
    { 
      //finds the module at the position according to the priority list.
      if (attackcount < 2)
      {
      for (int i=0; i<2; i++) //currently hardcoded to 1 attack will readd more later
      {
        if (WaterFlowController.initialModuleList[priority[i]].Attacked == false)
        {
          WaterFlowController.initialModuleList[priority[i]].Attack();
          Debug.Log("attacked at " + WaterFlowController.initialModuleList[priority[i]]);
          attackcount++;
          break;
        }
        //if attack tagets same target, loop to next in priority queue
         //break if attacked enough times?
        
      }
      }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
