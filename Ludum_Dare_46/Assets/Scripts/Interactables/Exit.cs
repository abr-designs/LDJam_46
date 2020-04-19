using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Exit : Interactable
{
    private Level _level;
    private int nextLevel;
    private void Start()
    {
        _level = GetComponentInParent<Level>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (CaveMan.isHoldingTorch && CaveMan.isTorchLit)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (!enabled)
            return;
        
        _level.LoadNext();
    }
}
