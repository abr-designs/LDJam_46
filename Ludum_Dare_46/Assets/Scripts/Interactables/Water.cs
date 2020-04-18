using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Interactable
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CaveMan.SetInWater(true);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CaveMan.SetInWater(false);
    }
}
