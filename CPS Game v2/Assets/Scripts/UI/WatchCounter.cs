using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchCounter : MonoBehaviour
{
    private Text counter;
    //Runs on startup, after user clicks start on title
    //Adds UpdateCounter to UpdateWatcher event list
    void Start()
    {                
        UIManager.current.onUpdateWatcherCountTrigger += UpdateCounter;     
        counter =  gameObject.GetComponent(typeof(Text)) as Text;        
        counter.text = GameController.current.NumAvailableCheckPlacements.ToString();
    }

    //Removes UpdateCounter from UpdateWatcher event list
    private void OnDestroy()
    {
        UIManager.current.onUpdateWatcherCountTrigger -= UpdateCounter;
    }

    //Called every time defender makes a move, updates number of watches placed text
    private void UpdateCounter()
    {
        counter.text = GameController.current.NumAvailableCheckPlacements.ToString();
    }
}
