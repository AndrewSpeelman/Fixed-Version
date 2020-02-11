using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    private Text counter;
    //Runs on startup, after user clicks start on title
    //Adds UpdateCounter to UpdateWatcher event list
    void Start()
    {                
        UIManager.current.onUpdateWatcherCountTrigger += UpdateCounter;     
        counter =  gameObject.GetComponent(typeof(Text)) as Text;
        counter.text = GameController.current.Turn.ToString();
    }

    
    private void OnDestroy()
    {
        UIManager.current.onUpdateWatcherCountTrigger -= UpdateCounter;
    }

    //Updates turn counter 
    //Runs every time defender places a watcher, for some reason
    private void UpdateCounter()
    {
        counter.text = GameController.current.Turn.ToString();
    }
}
