using UnityEngine;
using Zenject;

public class PlayerStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;
    private Player _player;
    private Animator _animator;
    private CharacterController _characterController;
    private PlayerClimber _playerClimber;

    //private InputPlayer _inputPlayer;
    //private PlayerInteractor _playerInteractor;

    //[Inject]
    //public void Construct(Player player,InputPlayer inputPlayer, PlayerInteractor playerInteractor)
    //{
    //    _player = player;
    //    //_inputPlayer = inputPlayer;
    //    //_playerInteractor = playerInteractor;
    //}


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerClimber = GetComponent<PlayerClimber>();
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new StateMachine();

        _stateMachine.AddState(new OnGroundState(_stateMachine));
        _stateMachine.AddState(new OnWallState(_stateMachine,_playerClimber));
        _stateMachine.AddState(new CrouchState(_stateMachine,_characterController, transform.GetChild(0).gameObject ));

        _stateMachine.SetState<OnGroundState>();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void SetState<T>() where T : State
    {
        _stateMachine.SetState<T>();
    }

    public State ReturnCurrentState()
    {
        return _stateMachine.CurrentState;
    }
}
