using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IFireAnim
{
    private bool done;
    private Action fireDoneCallback;
    private Animator _animator;
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void BurnOut(Action fireDoneCallback)
    {
        _animator.SetBool("Die", true);
        this.fireDoneCallback = fireDoneCallback;
        
        
    }

    public void FireAnimDone()
    {
        fireDoneCallback?.Invoke();

        done = true;
        _animator.enabled = false;
    }
}
