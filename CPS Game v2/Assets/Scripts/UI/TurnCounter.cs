using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    private Text counter;
    private int turn = 0;
    private int turnLimit = 0;
    //Runs on startup, after user clicks start on title
    //Adds UpdateCounter to UpdateWatcher event list
    void Start()
    {
        turn = 0;
        turnLimit = 0;
        UIManager.current.onUpdateWatcherCountTrigger += UpdateCounter;     
        counter =  gameObject.GetComponent(typeof(Text)) as Text;
        turn = GameController.current.DefenderTurns;
        turnLimit = GameController.current.TurnLimit;
        turn = turnLimit - turn;
        counter.text = turn.ToString();
    }

    
    private void OnDestroy()
    {
        UIManager.current.onUpdateWatcherCountTrigger -= UpdateCounter;
    }

    //Updates turn counter 
    //Runs every time defender places a watcher, for some reason
    private void UpdateCounter()
    {
        turn = GameController.current.DefenderTurns;
        turn = turnLimit - turn;
        counter.text = turn.ToString();
    }
}
