﻿using UnityEngine;
using UnityEngine.UI;

public class AttackerUI : MonoBehaviour
{
    private GameController gameController;
    public Text panelText;
    
    private void Start()
    {
        this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    /// <summary>
    /// Displays how many attacks the attacker has left
    /// </summary>
    private void Update()
    {
        if (this.panelText && this.gameController)
            this.panelText.text = "Available Attacks: " + this.gameController.NumAvailableAttacks;
    }
}
