using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    private Text counter;
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

    private void UpdateCounter()
    {
        counter.text = GameController.current.Turn.ToString();
    }
}
