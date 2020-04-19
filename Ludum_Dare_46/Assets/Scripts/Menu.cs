using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : Interactable
{
    [SerializeField]
    private float maxYValue;
    [SerializeField]
    private float minYValue;
    
    [SerializeField]
    private Transform musicBoulderTransform;
    [SerializeField]
    private Transform effectsBoulderTransform;
    
    private float currentMusicBoulderPos;
    private float currentEffectsBoulderPos;
    // Start is called before the first frame update
    // Update is called once per frame
    private void LateUpdate()
    {
        if (CheckIfChanged())
            UpdateVolume();
    }

    private bool CheckIfChanged()
    {
        var change = false;

        if (musicBoulderTransform.position.y != currentMusicBoulderPos)
        {
            change = true;
            currentMusicBoulderPos = musicBoulderTransform.position.y;
        }
        
        if (effectsBoulderTransform.position.y != currentEffectsBoulderPos)
        {
            change = true;
            currentEffectsBoulderPos = effectsBoulderTransform.position.y;
        }

        return change;
    }

    private void UpdateVolume()
    {
        AudioManager.SetMusicVolume(Mathf.InverseLerp(minYValue, maxYValue, currentMusicBoulderPos));
        AudioManager.SetEffectsVolume(Mathf.InverseLerp(minYValue, maxYValue, currentEffectsBoulderPos));
    }

}
