using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputController _playerControl;

    void Awake()
    {
        _playerControl = new InputController();
        OnEnable();

        _playerControl.AttackMap.Up.performed += OnAttackUp();
        _playerControl.AttackMap.Down.performed += OnAttackDown();
        _playerControl.AttackMap.Left.performed += OnAttackRight();
        _playerControl.AttackMap.Right.performed += OnAttackLeft();

        _playerControl.MoveMap.Up.performed += MoveUp();
        _playerControl.MoveMap.Down.performed += MoveDown();
        _playerControl.MoveMap.Left.performed += MoveLeft();
        _playerControl.MoveMap.Right.performed += MoveRight();
    }

    private Action<InputAction.CallbackContext> MoveRight()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> MoveLeft()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> MoveDown()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> MoveUp()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> OnAttackLeft()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> OnAttackRight()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> OnAttackDown()
    {
        throw new NotImplementedException();
    }

    private Action<InputAction.CallbackContext> OnAttackUp()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        _playerControl.Enable();
    }
}
