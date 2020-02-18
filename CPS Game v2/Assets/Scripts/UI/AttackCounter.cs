using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCounter : MonoBehaviour
{
    private Text counter;
    //Runs on startup, after user clicks start on title
    //Adds UpdateCounter to UpdateWatcher event list
    void Start()
    {
        UIManager.current.onUpdateAttackCountTrigger += UpdateCounter;
        counter = gameObject.GetComponent(typeof(Text)) as Text;
        counter.text = GameController.current.AttacksAvailable.ToString();
    }

    //Removes UpdateCounter from UpdateWatcher event list
    private void OnDestroy()
    {
        UIManager.current.onUpdateAttackCountTrigger -= UpdateCounter;
    }

    //Called every time defender makes a move, updates number of watches placed text
    private void UpdateCounter()
    {
        counter.text = GameController.current.AttacksAvailable.ToString();
    }
}