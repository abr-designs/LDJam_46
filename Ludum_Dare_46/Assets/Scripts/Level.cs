using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : Interactable
{
    public string LevelName;
    public int levelID;

    public AudioManager.MUSIC music = AudioManager.MUSIC.NONE;

    public Transform startPosition;

    public int nextLevel;

    public void LoadNext()
    {
        LevelManager.TryLoadNextLevel();
    }

}
