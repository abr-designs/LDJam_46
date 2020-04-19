using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : Interactable, IWalkAnimationEvent
{
    public void WalkAnimationEvent()
    {
        AudioManager.PlaySoundEffect(AudioManager.EFFECT.STEP, 0.6f);
    }
}
