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

        Dictionary<UnityEngine.Vector2, ConsoleKey> MoveDirections = new()
            {
                {UnityEngine.Vector2.up , ConsoleKey.UpArrow},
                {UnityEngine.Vector2.down , ConsoleKey.DownArrow},
                {UnityEngine.Vector2.left , ConsoleKey.LeftArrow},
                {UnityEngine.Vector2.right , ConsoleKey.RightArrow}
            };

        if (MoveDirections.TryGetValue(Direction, out var value))
        {
            _myGame.HandleMoveInput(value);
        }
    }

    private void OnAttackLeft()
    {
        var pos = _myGame.HandleAttackInput(ConsoleKey.D);
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
        if (pos != rogueLike.Vector2.Zero)
        {
            _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
            _attackParticle.Play();
        }
    }
}
