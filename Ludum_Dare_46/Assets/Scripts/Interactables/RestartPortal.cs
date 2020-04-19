using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartPortal : Interactable, IResartAnimationEvent
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        LevelManager.RestartLevel();
    }

    public void ResartAnimationEvent()
    {
        AudioManager.PlaySoundEffect(AudioManager.EFFECT.BEACON, 0.5f);
    }
}
