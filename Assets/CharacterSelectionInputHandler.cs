using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterSelectionInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput = null;
    private ExitTimeFilling _filler = null;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _filler = FindObjectOfType<ExitTimeFilling>();
    }

    private void OnLeaveStarted(InputAction.CallbackContext obj)
    {
        _filler.OnExitFillStart();
    }

    private void OnLeaveCanceled(InputAction.CallbackContext obj)
    {
        RemoveEvents();
        _filler.OnExitFillStop();

        Destroy(gameObject);
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        _filler.OnExitFillReset();
    }

    private void AddEvents()
    {
        _playerInput.actions["Leave"].started += OnLeaveStarted;
        _playerInput.actions["Leave"].canceled += OnLeaveCanceled;
        _playerInput.actions["Exit"].performed += OnExit;
    }

    private void RemoveEvents()
    {
        _playerInput.actions["Leave"].started -= OnLeaveStarted;
        _playerInput.actions["Leave"].canceled -= OnLeaveCanceled;
        _playerInput.actions["Exit"].performed -= OnExit;
    }

    private void OnEnable() => AddEvents();
    private void OnDisable() => RemoveEvents();

}
