using System;
using DG.Tweening;
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
    private float slideLenght = 10f ;
    private float horizontalVelocity = 0;
    private float dragPower = -9f;
    private bool isSliding = false;

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
        input.actions["Slide"].performed += OnSlideDown;

        _player.ProhibitClimb(1f);
        speed = playerSettings.walkSpeed;
        _player.isGravityActive = false;
    }

    private void OnSlideDown(InputAction.CallbackContext context)
    {
        if(speed == playerSettings.runSpeed)
        {
            DoSlide();
        }
    }

    private void DoSlide()
    {
        GameObject model = _player.transform.GetChild(0).gameObject;
        controller.height = playerSettings.crouchHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = -90f;
        model.transform.rotation = Quaternion.Euler(newRotation);
        horizontalVelocity = MathF.Sqrt(playerSettings.slideLenght * dragPower * -2);
        isSliding = true;
        //controller.Move(_player.transform.position + (_player.transform.forward * slideLenght), 1.5f);
        //_player.transform.DOMove(_player.transform.position + (_player.transform.forward * slideLenght),1.5f);
        
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
        if (isSliding)
        {
            UseDrag(); 
        }
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
    private void UseDrag()
    {
        Debug.Log($"horizontalVelocity {horizontalVelocity}");
        horizontalVelocity += dragPower * Time.deltaTime;
        controller.Move(_player.transform.forward * horizontalVelocity * Time.deltaTime);
        if (horizontalVelocity < 0)
        {
            isSliding = false;
            Stand();
        }
    }

    private void Stand()
    {
        GameObject model = _player.transform.GetChild(0).gameObject;
        controller.height = playerSettings.standHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = 0f;
        model.transform.rotation = Quaternion.Euler(newRotation);
    }
}
