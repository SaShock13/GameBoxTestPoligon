using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    private PlayerInput _input;
    private CharacterController controller;
    private float speed ;
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float sprintSpeed = 8;
    [SerializeField] private float jumpHieght = 2;
    private bool lookLeft = true;
    private Vector2 moveValue;
    private bool isGravityActive = true;
    public float verticalVelocity;
    private float gravity = -9.8f;
    private float characterCrouchHeight;
    private PlayerStateMachine stateMachine;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        speed = walkSpeed;
        controller = GetComponent<CharacterController>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
        moveValue.y = 0;
        if (moveValue.x > 0 && lookLeft) 
        {
            lookLeft = false;
            SwitchModelDirection(false);
        } else if(moveValue.x < 0 && !lookLeft)
        {
            lookLeft = true;
            SwitchModelDirection(true);
        }

        //Debug.Log($"Player move to vector  {moveValue}");
    }

    private void SwitchModelDirection(bool onRight)
    {
        Debug.Log($"Switch dir {this}");
        var newRotation = transform.rotation.eulerAngles;
        newRotation.y = onRight ? -90 : 90 ;
        transform.rotation = Quaternion.Euler( newRotation);
    }

    public void OnJump(InputAction.CallbackContext context)
    {        
        if (controller.isGrounded && stateMachine.ReturnCurrentState() is OnGroundState)
        {
            //StartCoroutine(TryHang());
            //animator.SetTrigger("Jump 0");
            verticalVelocity = MathF.Sqrt(jumpHieght * gravity * -2);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (controller.isGrounded) // спринт только на земле
            {
                speed = sprintSpeed;
                Debug.Log($"On Sprint {this}"); 
            }
        }
        else if (context.canceled)
        {
            speed = walkSpeed;
            Debug.Log($"OFF Sprint {this}");
        }
    }
    public void OffSprint(InputAction.CallbackContext context)
    {
        Debug.Log($"OFF Sprint {this}");
        speed = walkSpeed;
    }




    public void OnCrouchDown(InputAction.CallbackContext context)
    {
        if (controller.isGrounded )
        {
            if (!(stateMachine.ReturnCurrentState() is CrouchState))
            {
                stateMachine.SetState<CrouchState>();
            }
            else
            {
                stateMachine.SetState<OnGroundState>();
            }
        }
    }

    private void Update()
    {
        verticalVelocity = verticalVelocity < gravity ? gravity : verticalVelocity;
        if (controller.isGrounded)
        {
            //animator.SetBool("Grounded", true);
            //verticalVelocity = -2f;
        }
        else
        {
            //animator.SetBool("Grounded", false);
        }
        controller.Move(moveValue * Time.deltaTime * speed);
        UseGravity();
    }
    private void UseGravity()
    {
        if (isGravityActive)
        {
            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
    }

}
