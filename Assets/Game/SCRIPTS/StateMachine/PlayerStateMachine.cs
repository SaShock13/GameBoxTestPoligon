using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;
    private PlayerInput playerInput;
    private Player _player;
    private Animator _animator;
    private CharacterController _characterController;
    private PlayerClimber _playerClimber;
    [SerializeField] private PlayerSettings _playerSettings;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _playerClimber = GetComponent<PlayerClimber>();
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
        _stateMachine = new StateMachine();
        _stateMachine.AddState(new OnGroundState(_stateMachine, _playerClimber, _characterController, _player, playerInput, _playerSettings));
        _stateMachine.AddState(new OnWallState(_stateMachine,_playerClimber,_characterController,_player,playerInput));
        _stateMachine.AddState(new CrouchState(_stateMachine,_characterController, transform.GetChild(0).gameObject , playerInput, _player, _playerSettings));
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
