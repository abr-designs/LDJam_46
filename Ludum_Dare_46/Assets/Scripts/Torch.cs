using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    private Collider2D rigidbodyCollider;
    private static CaveManController _caveManController;

    private void Start()
    {
        if (!_caveManController)
            _caveManController = FindObjectOfType<CaveManController>();
    }

    public void Init(Collider2D ignoreCollider)
    {
        Physics2D.IgnoreCollision(ignoreCollider, rigidbodyCollider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        _caveManController.canPickupTorch = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        _caveManController.canPickupTorch = false;
    }
}
