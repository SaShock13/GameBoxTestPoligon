using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchState : State
{
    private Animator _animator;
    private CharacterController _characterController;
    private PlayerInput input;
    private Player player;
    private float crouchHeight = 1f;
    private float standHeight = 2f;
    private float crouchCenterHeight = 0.5f;
    private float standCenterHeight = 0f;
    private GameObject model;
    private Vector2 moveValue;
    private float verticalVelocity = 0;
    private bool isGravityActive = true;
    private float gravity = -9.8f;
    private float speed = 3f;
    private bool lookLeft ;
    private PlayerSettings settings;
    

    public CrouchState(StateMachine stateMachine, CharacterController controller, 
                        GameObject model, PlayerInput input, 
                        Player player, PlayerSettings settings) : base(stateMachine)
    {
        this.player = player;
        _characterController = controller;
        this.input = input;
        this.model = model;
        this.settings = settings;
    }

    public override void Enter()
    {
        input.actions["Move"].performed += Move_performed;
        input.actions["Move"].canceled += Move_canceled;
        input.actions["Crouch"].performed += OnCrouchDown;


        Debug.Log($"Enter Crouch {this}");
        base.Enter();
        _characterController.height = crouchHeight;
        //var newCenter = _characterController.center;
        //newCenter.y = crouchCenterHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = 90f;
        model.transform.rotation = Quaternion.Euler(newRotation);
        //_characterController.center = newCenter;
    }

    private void OnCrouchDown(InputAction.CallbackContext context)
    {
        
            if (IsCanStand())
            {
                stateMachine.SetState<OnGroundState>();
            } 
    }

    /// <summary>
    /// todo написать логику проверки упирания в потолок
    /// </summary>
    /// <returns></returns>
    private bool IsCanStand()
    {

        return true;
    }

    private void Move_canceled(InputAction.CallbackContext context)
    {
        moveValue = Vector2.zero;
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
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

    private void SwitchModelDirection(bool onRight)
    {
        Debug.Log($"Switch dir {this}");
        var newRotation = player.transform.rotation.eulerAngles;
        newRotation.y = onRight ? -90 : 90;
        player.transform.rotation = Quaternion.Euler(newRotation);
    }


    public override void Exit()
    {
        Debug.Log($"Exit Crouch {this}");
        base.Exit();
        _characterController.height = standHeight;
        //var newCenter = _characterController.center;
        //newCenter.y = standCenterHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = 0f;
        model.transform.rotation = Quaternion.Euler(newRotation);
        input.actions["Move"].performed -= Move_performed;
        input.actions["Move"].canceled -= Move_canceled;
        input.actions["Crouch"].performed -= OnCrouchDown;
    }


    public override void Update()
    {
        if (moveValue != Vector2.zero)
        {
            _characterController.Move(moveValue * Time.deltaTime * settings.crouchSpeed );
        }
        UseGravity();
    }

    private void UseGravity()
    {
        verticalVelocity = verticalVelocity < gravity ? gravity : verticalVelocity;
        if (isGravityActive)
        {
            verticalVelocity += gravity * Time.deltaTime;
            _characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
    }
}
