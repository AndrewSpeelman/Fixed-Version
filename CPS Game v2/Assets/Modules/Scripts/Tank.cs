using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Module
{
    public Pump OutFlowingPump;

    public GameObject overFlowSprite;


    public int Fill {
        get {
            return WaterList.Count;
        }
     }

}