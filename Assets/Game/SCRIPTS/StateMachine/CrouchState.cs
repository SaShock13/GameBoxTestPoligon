using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchState : State
{
    private Animator _animator;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private float crouchHeight = 1f;
    private float standHeight = 2f;
    private float crouchCenterHeight = 0.5f;
    private float standCenterHeight = 0f;
    private GameObject model;

    public CrouchState(StateMachine stateMachine, CharacterController controller, GameObject model) : base(stateMachine)
    {
        _characterController = controller;
        this.model = model;
    }

    public override void Enter()
    {

        Debug.Log($"Enter Crouch {this}");
        base.Enter();
        _characterController.height = crouchHeight;
        var newCenter = _characterController.center;
        newCenter.y = crouchCenterHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = 90f;
        model.transform.rotation = Quaternion.Euler(newRotation);
        //_characterController.center = newCenter;
    }

    public override void Exit()
    {
        Debug.Log($"Exit Crouch {this}");
        base.Exit();
        _characterController.height = standHeight;
        var newCenter = _characterController.center;
        newCenter.y = standCenterHeight;
        var newRotation = model.transform.rotation.eulerAngles;
        newRotation.x = 0f;
        model.transform.rotation = Quaternion.Euler(newRotation);
        //_characterController.center = newCenter;
    }


}
