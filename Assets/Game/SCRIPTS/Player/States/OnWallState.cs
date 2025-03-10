using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnWallState : State
{
    private Player _player;
    private PlayerClimber climber;
    private CharacterController controller;
    private PlayerInput input;
    private Vector2 moveValue;
    private float climbSpeed = 2;
    private bool isGrounded = false;

    public OnWallState(StateMachine stateMachine, PlayerClimber climber, CharacterController controller, Player player, PlayerInput input) : base(stateMachine)
    {
        this.climber = climber;
        this.controller = controller;
        this.input = input;
        _player = player;
    }
    public override void Enter()
    {        
        input.actions["Move"].performed += OnWallState_performed;
        input.actions["Move"].canceled += OnWallState_canceled;
        input.actions["Jump"].performed += OnJump;
        climber.OnLedgeRichedEvent += OnLedgeRiched;
        Debug.Log("OnWall State Enter");
        //climber.enabled = false;
        _player.isGravityActive = false;
        moveValue = Vector2.zero;
        isGrounded = false;
    }

    private void OnLedgeRiched(float ledgeY)
    {
        _player.transform.DOMove(new Vector3(
                                    _player.transform.position.x, 
                                    _player.transform.position.y + 1.5f, 
                                    _player.transform.position.z) + 
                                    _player.transform.forward, 1f);
        stateMachine.SetState<OnGroundState>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        stateMachine.SetState<OnGroundState>();
    }

    private void OnWallState_canceled(InputAction.CallbackContext context)
    {
        moveValue = Vector2.zero;
    }

    private void OnWallState_performed(InputAction.CallbackContext context)
    {        
        moveValue = context.ReadValue<Vector2>();
        moveValue.x = 0;
        if(moveValue.y < 0 && controller.isGrounded)
        {
            stateMachine.SetState<OnGroundState>();
        }
        Debug.Log($"OnWallState_performed {this}"); 
    }

    public override void Exit()
    {
        isGrounded = true;
        Debug.Log("OnWall State Exit");
        //climber.enabled = true;
        _player.isGravityActive = true;
        input.actions["Move"].performed -= OnWallState_performed;
        input.actions["Move"].canceled -= OnWallState_canceled;
        input.actions["Jump"].performed -= OnJump;
        climber.OnLedgeRichedEvent -= OnLedgeRiched;
        _player.ProhibitClimb(1f);
    }

    public override void Update()
    {
        controller.Move(moveValue * Time.deltaTime * climbSpeed);
    }
}
