using UnityEngine;

public class OnGroundState : State
{
    //private InputPlayer _input;
    //private Player _player;
    //private PlayerInteractor _interactor;

    public OnGroundState(StateMachine stateMachine) : base(stateMachine)
    {
        //_input = input;
        //_player = player;
        //_interactor = interactor;
    }

    //public override void Enter()
    //{
    //    InputPlayer.OnTorchPressedEvent += _player.GetTorch;
    //    InputPlayer.OnAttackEvent += _player.Attack;
    //    InputPlayer.OnJumpEvent += _player.Jump;
    //    InputPlayer.OnMoveEvent += _player.MovePlayer;
    //    InputPlayer.OnInteractEvent += _interactor.Interact;
    //    //Debug.Log("Onground State Enter");
    //}

    //public override void Exit()
    //{
    //    //Debug.Log("Onground State Exit");
    //    InputPlayer.OnTorchPressedEvent -= _player.GetTorch;
    //    InputPlayer.OnAttackEvent -= _player.Attack;
    //    InputPlayer.OnJumpEvent -= _player.Jump;
    //    InputPlayer.OnMoveEvent -= _player.MovePlayer;
    //    InputPlayer.OnInteractEvent -= _interactor.Interact;
    //}

    //public override void Update()
    //{
    //    _player.LookCamera();
    //}
}
