using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool fireExists;
    
    private List<IFlammable> activeFlammables;
    private CaveManController _caveManController;

    private void Start()
    {
        _caveManController = FindObjectOfType<CaveManController>();
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


    public void RegisterFire(IFlammable flammable)
    {
        if (activeFlammables == null)
            activeFlammables = new List<IFlammable>();
        
        if(activeFlammables.Contains(flammable))
            throw new Exception("Flammable already registered");
        
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

}
