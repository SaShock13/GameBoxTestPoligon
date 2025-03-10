using System;
using UnityEngine;
using Zenject;

public class PlayerClimber : MonoBehaviour
{
    [SerializeField] Transform bottomPoint;
    [SerializeField] Transform topPoint;
    [SerializeField] private float wallRayDistance;
    [SerializeField] private LayerMask wallRayLayerMask;

    public Action<float> OnLedgeRichedEvent;

    private Player _player;
    private PlayerStateMachine _stateMachine;
    public bool isOnWall = false;
    public bool isOnLedge = false;

    private void Start()
    {
        _player = GetComponent<Player>();
        _stateMachine = GetComponent<PlayerStateMachine>();
    }

    public bool IsCanClimb()
    {
        Ray climableWallCheckRay = new Ray(bottomPoint.position, bottomPoint.forward);
        Ray ledgeCheckRay = new Ray(topPoint.position, bottomPoint.forward);

        if (Physics.Raycast(climableWallCheckRay, out RaycastHit hit, wallRayDistance, wallRayLayerMask) && ! isOnWall)
        {
            if (_player.isAllowedToClimb) // если можно карабкаться
            {
                isOnWall = true;
                _stateMachine.SetState<OnWallState>();
            } 
        }
        else isOnWall = false;

        if (isOnWall && !(Physics.Raycast(ledgeCheckRay, out RaycastHit topHit, wallRayDistance * 2, wallRayLayerMask)))
        {            
            isOnLedge = true;
            OnLedgeRichedEvent?.Invoke(topHit.point.y);
            return true;
        }
        else
        {
            isOnLedge = false;
        }
        return false;
    }

    private void Update()
    {
        IsCanClimb();
        Debug.DrawRay(bottomPoint.position, bottomPoint.forward * wallRayDistance, Color.blue);
        Debug.DrawRay(topPoint.position, topPoint.forward * wallRayDistance * 2f, Color.red);
    }
}
