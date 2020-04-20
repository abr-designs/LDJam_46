using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : Interactable
{
    //================================================================================================================//
    [SerializeField]
    private bool fireExists;

    [SerializeField] private GameObject restartLevelBeaconPrefab;

    [SerializeField] 
    private GameObject levelPrefabObject;

    /*[SerializeField]
    private bool startWithMenu;*/

    [SerializeField] private Level menuLevel;
    
    private List<IFlammable> activeFlammables;
    private CaveManController _caveManController;

    [SerializeField]
    private GameObject gameParentObject;

    [SerializeField] 
    private int currentLevelIndex;

    
    private List<Level> Levels;
    private GameObject activeLevel;

    //================================================================================================================//

    private void Start()
    {
        Levels = new List<Level>();
        var levels = levelPrefabObject.GetComponentsInChildren<Level>(true);
        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
            
            if(level.isActive)
                Levels.Add(level);
        }


        _caveManController = FindObjectOfType<CaveManController>();
        fireExists = true;


        LoadLevel(currentLevelIndex);
    }

    private void Update()
    {
        CheckForOutOfBounds();
        
        if(Input.GetKeyDown(KeyCode.Escape))
            RestartLevel();
        
        //If we lost, dont keep checking
        if (!fireExists)
            return;
        
        //If the player currently has fire, we dont need to check the level for fire
        if (_caveManController.isTorchLit && _caveManController.isHoldingTorch)
        {
            fireExists = true;
            return;
        }
        
        //TODO Check registered list of fire

        if (activeFlammables != null && activeFlammables.Count > 0)
        {
            if (activeFlammables.Any(activeFlammable => activeFlammable.isOnFire))
            {
                fireExists = true;
                return;
            }
        }

        //TODO Trigger some event to say the level has failed
        HasFailed();
        Debug.LogError("Nothing is on fire, you lose");
    }
    
    //================================================================================================================//

    public void RegisterFire(IFlammable flammable)
    {
        if (activeFlammables == null)
            activeFlammables = new List<IFlammable>();

        if (activeFlammables.Contains(flammable))
        {
            //Debug.LogError("Flammable already registered");
            return;
        }
        
        activeFlammables.Add(flammable);
    }
    public void UnRegisterFire(IFlammable flammable)
    {
        if (activeFlammables == null)
            return;

        if (!activeFlammables.Contains(flammable))
        {
            return;
            //throw new Exception("Flammable was never registered");
        }
        
        activeFlammables.Remove(flammable);
    }
    
    //================================================================================================================//

    private void CheckForOutOfBounds()
    {
        var currentPos = _caveManController.CurrentPosition;

        if (Mathf.Abs(currentPos.x) > 7.5f || Mathf.Abs(currentPos.y) > 7.5f)
            RestartLevel();
        
    }
    
    public void ForceFailed()
    {
        HasFailed();
    }
    private void HasFailed()
    {
        fireExists = false;
        AudioManager.PlayMusic(AudioManager.MUSIC.NONE);

        var temp = Instantiate(restartLevelBeaconPrefab, Levels[currentLevelIndex].startPosition.position,
            Quaternion.identity);
        
        temp.transform.parent = activeLevel.transform;
    }

    [ContextMenu("Restart")]
    public void RestartLevel()
    {
        _caveManController.Reset();
        fireExists = true;
        LoadLevel(currentLevelIndex);
        
    }
    
    public void RestartGame()
    {
        _caveManController.Reset();
        fireExists = true;
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
        
    }

    public void SetParentToLevel(Transform child)
    {
        if (!activeLevel)
            return;

        child.parent = activeLevel.transform;
    }
    
    
    //================================================================================================================//

    public void TryLoadNextLevel()
    {
            currentLevelIndex++;
            LoadLevel(currentLevelIndex);
    }
    /*public void TryLoadLevel(int index)
    {
            LoadLevel(index);
    }*/
    
    private void LoadLevel(int index)
    {
        if(activeLevel)
            unloadLevel();

        var level = index < 0 ? menuLevel : Levels[index];

        currentLevelIndex = index;
        //Debug.Log(Levels[index].startPosition.position);
        _caveManController.SetPosition(level.startPosition.position);
        _caveManController.SetIsHoldingTorch(level.startWithTorch);
        activeLevel = Instantiate(level.gameObject, gameParentObject.transform, true);
        
        AudioManager.PlayMusic(level.music);
        
        
        activeLevel.SetActive(true);

        gameObject.name = $"[GAME MANAGER - LEVEL {index}]";

    }
    private void LoadLevel(Level level)
    {
        if(activeLevel)
            unloadLevel();

        currentLevelIndex = -1;
        //Debug.Log(Levels[index].startPosition.position);
        _caveManController.SetPosition(level.startPosition.position);
        _caveManController.SetIsHoldingTorch(level.startWithTorch);
        activeLevel = Instantiate(level.gameObject, gameParentObject.transform, true);
        
        AudioManager.PlayMusic(level.music);
        
        
        activeLevel.SetActive(true);
    }
    private void LoadLevelWithID(int ID)
    {
        
    }

    private void unloadLevel()
    {
        //Debug.Log($"Unload {activeLevel.name}");
        Destroy(activeLevel);
    }

}
