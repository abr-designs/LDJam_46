using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : Interactable
{
    private bool checkDirection;
    private bool ignoreInput;
    private const float timeCheck = 0.25f;

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

        currentInput = CaveMan.moveDirection;

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
        var test = Physics2D.Raycast(transform.position, dir, 0.8f);

        if (test.transform)
            return;

        //TODO Move the rock in that direction
        //ignoreInput = true;
        //transform.position += dir * 0.4f;
        var target = transform.position + dir * 0.8f;
        StartCoroutine(MoveBoulderCoroutine(target, 0.3f));

        lastInput = currentInput = Vector2Int.zero;
    }

    private bool isMoving;
    private IEnumerator MoveBoulderCoroutine(Vector2 targetPosition, float time)
    {
        ignoreInput = true;
        var _t = 0f;
        var startPosition = transform.position;
        
        AudioManager.PlaySoundEffect(AudioManager.EFFECT.PUSH, 0.3f);
        
        while (_t < time)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, _t / time);

            _t += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        ignoreInput = false;
    }
}
