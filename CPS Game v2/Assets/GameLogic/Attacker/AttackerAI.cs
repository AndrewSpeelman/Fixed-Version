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
    //add Priority list
    public List<int> Priority = new List<int>(); 
    int attackcount = 0; //testing counter to make sure its not attacking more than 2 total times
    public void Attack()
    { 
    int i = 0;
      while(Priority.Count >= 1) 
      {
        if (WaterFlowController.initialModuleList[Priority[i]].Attacked == false
            && WaterFlowController.initialModuleList[Priority[i]] is Pipe)
        {
          WaterFlowController.initialModuleList[Priority[i]].Attack();
          Debug.Log("attacked at " + WaterFlowController.initialModuleList[Priority[i]]);
          attackcount++;
          i++;
          break;
        }
        else
        {
          FronttoBack(Priority);
        }
            //if attack tagets same target, loop to next in Priority queue
             //break if attacked enough times?
        
      }

    }

   public void FronttoBack (List<int> Priority)
   { // moves the front of hte priority list to the back -- only makes so much variation.
     int tmp = Priority[0];
     Priority.RemoveAt(0);
     Priority.Add(tmp);
   }
   
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
