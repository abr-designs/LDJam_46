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

    [SerializeField]
    private bool startWithMenu;

    [SerializeField] private Level menuLevel;
    
    private List<IFlammable> activeFlammables;
    private CaveManController _caveManController;

    [SerializeField]
    private GameObject gameParentObject;

    [SerializeField] 
    private int currentLevelIndex;
    [SerializeField]
    private List<Level> Levels;
    private GameObject activeLevel;
    
    //================================================================================================================//

    private void Start()
    {
        foreach (var level in Levels)
        {
            level.gameObject.SetActive(false);
        }
        _caveManController = FindObjectOfType<CaveManController>();
        fireExists = true;

        if (startWithMenu)
            LoadLevel(menuLevel);
        else
            LoadLevel(currentLevelIndex);
    }

    private void Update()
    {
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
            if (activeFlammables.Any(activeFlammable => activeFlammable.isOnFire()))
            {
                fireExists = true;
                return;
            }
        }

        //TODO Trigger some event to say the level has failed
        fireExists = false;
        Debug.LogError("Nothing is on fire, you lose");
    }
    
    //================================================================================================================//

    public void RegisterFire(IFlammable flammable)
    {
        if (activeFlammables == null)
            activeFlammables = new List<IFlammable>();

        if (activeFlammables.Contains(flammable))
        {
            Debug.LogError("Flammable already registered");
            return;
        }
        
        activeFlammables.Add(flammable);
    }
    public void UnRegisterFire(IFlammable flammable)
    {
        if (activeFlammables == null)
            return;
        
        if(!activeFlammables.Contains(flammable))
            throw new Exception("Flammable was never registered");
        
        activeFlammables.Remove(flammable);
    }
    
    //================================================================================================================//

    [ContextMenu("Restart")]
    public void RestartLevel()
    {
        _caveManController.Reset();
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
    public void TryLoadLevel(int index)
    {
            LoadLevel(index);
    }
    
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
