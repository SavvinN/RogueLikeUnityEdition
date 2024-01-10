using rogueLike;
using rogueLike.GameObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputController _playerControl;
    public static ParticleSystem _attackParticle;
    public static Game _myGame;

    void Awake()
    {
        _playerControl = new InputController();
        OnEnable();

        _playerControl.AttackMap.Up.performed += context => OnAttackUp();
        _playerControl.AttackMap.Down.performed += context => OnAttackDown();
        _playerControl.AttackMap.Left.performed += context => OnAttackRight();
        _playerControl.AttackMap.Right.performed += context => OnAttackLeft();
    }

    private void Update()
    {
        var Direction = _playerControl.MoveMap.Move.ReadValue<UnityEngine.Vector2>();

        if (Direction == UnityEngine.Vector2.up)
            _myGame.HandleMoveInput(ConsoleKey.UpArrow);
        if (Direction == UnityEngine.Vector2.down)
            _myGame.HandleMoveInput(ConsoleKey.DownArrow);
        if (Direction == UnityEngine.Vector2.left)
            _myGame.HandleMoveInput(ConsoleKey.LeftArrow);
        if (Direction == UnityEngine.Vector2.right)
            _myGame.HandleMoveInput(ConsoleKey.RightArrow);
    }

    private void OnAttackLeft()
    {
        var pos  = _myGame.HandleAttackInput(ConsoleKey.D);
        TryPlayParticle(pos);
    }

    private void OnAttackRight()
    {
        var pos = _myGame.HandleAttackInput(ConsoleKey.A);
        TryPlayParticle(pos);
    }

    private void OnAttackDown()
    {
        var pos = _myGame.HandleAttackInput(ConsoleKey.S);
        TryPlayParticle(pos);
    }

    private void OnAttackUp()
    {
        var pos = _myGame.HandleAttackInput(ConsoleKey.W);
        TryPlayParticle(pos);
    }

    private void OnEnable()
    {
        _playerControl.Enable();
    }

    private void OnDisable()
    {
        _playerControl.Disable();
    }

    private void TryPlayParticle(rogueLike.Vector2 pos)
    {
        if(pos != rogueLike.Vector2.Zero)
        {
            _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
            _attackParticle.Play();
        }
    }
}
