using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGroundState : State
{
    private Player _player;
    private PlayerClimber climber;
    private CharacterController controller;
    private PlayerInput input;
    private Vector2 moveValue;
    private bool lookLeft = true;
    public float verticalVelocity = 0;
    private bool isGravityActive = true;
    private float gravity = -9.8f;
    private float jumpHieght = 2.5f;
    private float speed;
    private PlayerSettings playerSettings;

    public OnGroundState(StateMachine stateMachine, PlayerClimber climber, CharacterController controller, 
                        Player player, PlayerInput input ,PlayerSettings settings) : base(stateMachine)
    {
        this.playerSettings = settings;
        this.climber = climber;
        this.controller = controller;
        this.input = input;
        _player = player;
    }
    // todo сделать цепляние за стену не сразу при входе в состояние
    public override void Enter()
    {
        input.actions["Move"].performed += Move_performed;
        input.actions["Move"].canceled += Move_canceled;
        input.actions["Jump"].performed += OnJump;
        input.actions["Sprint"].performed += OnSprint;
        input.actions["Sprint"].canceled += OffSprint;
        input.actions["Crouch"].performed += OnCrouchDown;
        _player.ProhibitClimb(1f);
        speed = playerSettings.walkSpeed;
        _player.isGravityActive = false;
    }

    private void Move_canceled(InputAction.CallbackContext context)
    {
        moveValue = Vector2.zero;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        if(!_player.isAllowedToClimb) _player.isAllowedToClimb = true;
        moveValue = context.ReadValue<Vector2>();
        moveValue.y = 0;
        if (moveValue.x > 0 && lookLeft)
        {
            lookLeft = false;
            SwitchModelDirection(false);
        }
        else if (moveValue.x < 0 && !lookLeft)
        {
            lookLeft = true;
            SwitchModelDirection(true);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = MathF.Sqrt(playerSettings.jumpHeight * gravity * -2);
            Debug.Log($"verticalVelocity {verticalVelocity}");
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {        
        if (controller.isGrounded) // спринт только на земле
        {
            speed = playerSettings.runSpeed;
        }       
    }

    public void OffSprint(InputAction.CallbackContext context)
    {
        speed = playerSettings.walkSpeed;
    }

    public void OnCrouchDown(InputAction.CallbackContext context)
    {        
        stateMachine.SetState<CrouchState>();
    }

    private void SwitchModelDirection(bool onRight)
    {
        Debug.Log($"Switch dir {this}");
        var newRotation = _player.transform.rotation.eulerAngles;
        newRotation.y = onRight ? -90 : 90;
        _player.transform.rotation = Quaternion.Euler(newRotation);
    }

    public override void Exit()
    {
        input.actions["Move"].performed -= Move_performed;
        input.actions["Move"].canceled -= Move_canceled;
        input.actions["Jump"].performed -= OnJump;
        input.actions["Sprint"].performed -= OnSprint;
        input.actions["Sprint"].canceled -= OffSprint;
        input.actions["Crouch"].performed -= OnCrouchDown;
    }

    public override void Update()
    {
        if (moveValue!=Vector2.zero)
        {
            controller.Move(moveValue * Time.deltaTime * speed); 
        }
        UseGravity();
    }

    private void UseGravity()
    {
        verticalVelocity = verticalVelocity < gravity ? gravity : verticalVelocity;
        if (isGravityActive)
        {
            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }    
    }
}
