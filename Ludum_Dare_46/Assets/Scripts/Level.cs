using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : Interactable
{
    public bool isActive = true;
    
    public string LevelName;
    // public int levelID;

    public bool startWithTorch = true;

    public AudioManager.MUSIC music = AudioManager.MUSIC.NONE;

    public Transform startPosition;

    //public int nextLevel;

    public void LoadNext()
    {
        LevelManager.TryLoadNextLevel();
    }

}
