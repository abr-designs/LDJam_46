using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedBush : Interactable, ITreeFaded
{
    private Action OnBushFadedCallback;

    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void BreakBush(Action OnBushFadedCallback)
    {
        this.OnBushFadedCallback = OnBushFadedCallback;
        _animator.SetFloat("Play", 1f);
        
        AudioManager.PlaySoundEffect(AudioManager.EFFECT.TO_ASH);
    }
    public void TreeFadedAnim()
    {
        OnBushFadedCallback?.Invoke();
    }
}
