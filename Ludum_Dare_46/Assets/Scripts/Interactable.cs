using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected static CaveManController CaveMan
    {
        get
        {
            if (!_caveManController)
                _caveManController = FindObjectOfType<CaveManController>();

            return _caveManController;
        }
    }
    private static CaveManController _caveManController;
}
