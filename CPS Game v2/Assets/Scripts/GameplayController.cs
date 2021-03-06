﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{

    /// <summary>
    //  All Gameplay related logic and prefabds are handled here related mechanics are done here. 
    //  Currently
    //  GameController has some of the logic so transfer over later
    //  GameController should be handling macro logic,
    /// </summary>

    private static int level = 0;
    public const int NUM_LEVELS = 3;

    public static GameplayController current;
    [SerializeField]
    public  GameObject Defender_Generic;

    //Actually does nothing/is inaccessible ???
    void Awake()
    {
        if(current == null)
        {
            current=this;

        }
        else
         Destroy(gameObject);    
    }

    public static int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }
}
