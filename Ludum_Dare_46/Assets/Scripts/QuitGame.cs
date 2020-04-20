using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuitGame : Interactable
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        #if UNITY_WEBGL
        LevelManager.RestartGame();
        #else
        if (CaveMan.isHoldingTorch && CaveMan.isTorchLit)
            Application.Quit();
        #endif
    }

}
