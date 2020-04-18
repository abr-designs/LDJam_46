using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private static CaveManController CaveManController
    {
        get
        {
            if (!_caveManController)
                _caveManController = FindObjectOfType<CaveManController>();

            return _caveManController;
        }
    }

    private static CaveManController _caveManController;

    private bool checkDirection;
    private bool ignoreInput;
    private const float timeCheck = 0.4f;

    private float timer = 0f;
    private Vector2Int lastInput;
    private Vector2Int currentInput;

    private new Transform transform;

    private void Start()
    {
        transform = gameObject.transform;
    }

    private void Update()
    {
        if (ignoreInput)
            return;
        
        if (!checkDirection)
            return;

        currentInput = CaveManController.moveDirection;

        if (currentInput == Vector2Int.zero)
        {
            timer = 0f;
            return;
        }


        if (currentInput == lastInput)
        {
            timer += Time.deltaTime;

            if (timer >= timeCheck)
            {
                TryMoveDirection(lastInput);
            }
        }
        else
        {
            lastInput = currentInput;
            timer = 0f;
        }

        //Debug.Log(CaveManController.moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        checkDirection = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        checkDirection = false;
    }

    private void TryMoveDirection(Vector2Int direction)
    {
        //TODO Need to check if legal to move to next position
        var dir = new Vector3(direction.x, direction.y);
        var test = Physics2D.Raycast(transform.position, dir, 0.4f);

        if (test.transform)
            return;

        //TODO Move the rock in that direction
        //ignoreInput = true;
        transform.position += dir * 0.4f;

        lastInput = currentInput = Vector2Int.zero;
    }
}
