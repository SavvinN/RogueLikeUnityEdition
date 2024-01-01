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
    public static Game myGame;

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
            myGame.HandleMoveInput(myGame.MyWorld.GetPlayer(), ConsoleKey.UpArrow);
        if (Direction == UnityEngine.Vector2.down)
            myGame.HandleMoveInput(myGame.MyWorld.GetPlayer(), ConsoleKey.DownArrow);
        if (Direction == UnityEngine.Vector2.left)
            myGame.HandleMoveInput(myGame.MyWorld.GetPlayer(), ConsoleKey.LeftArrow);
        if (Direction == UnityEngine.Vector2.right)
            myGame.HandleMoveInput(myGame.MyWorld.GetPlayer(), ConsoleKey.RightArrow);
    }

    private void OnAttackLeft()
    {
        var pos  = myGame.HandleAttackInput(myGame.MyWorld.GetPlayer(), ConsoleKey.D);
        _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
        _attackParticle.Play();
    }

    private void OnAttackRight()
    {
        var pos = myGame.HandleAttackInput(myGame.MyWorld.GetPlayer(), ConsoleKey.A);
        _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
        _attackParticle.Play();
    }

    private void OnAttackDown()
    {
        var pos = myGame.HandleAttackInput(myGame.MyWorld.GetPlayer(), ConsoleKey.S);
        _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
        _attackParticle.Play();
    }

    private void OnAttackUp()
    {
        var pos = myGame.HandleAttackInput(myGame.MyWorld.GetPlayer(), ConsoleKey.W);
        _attackParticle.transform.position = new Vector3(pos.X, 0, pos.Y);
        _attackParticle.Play();
    }

    private void OnEnable()
    {
        _playerControl.Enable();
    }

    private void OnDisable()
    {
        _playerControl.Disable();
    }
}
