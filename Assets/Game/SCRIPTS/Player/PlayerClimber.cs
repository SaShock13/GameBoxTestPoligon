using UnityEngine;
using Zenject;

public class PlayerClimber : MonoBehaviour
{
    [SerializeField] Transform bottomPoint;
    [SerializeField] Transform topPoint;
    [SerializeField] private float hangRayDistance;
    [SerializeField] private LayerMask hangRayLayerMask;

    private Player _player;
    private PlayerStateMachine _stateMachine;
    public bool isOnWall = false;
    public bool isOnLedge = false;
    //public bool isTryToHang = false;
    //private Vector3 thirdRayOrigin;
    //private float twoRaysYDistance;
    //private Vector3 hangNormal;

    //[Inject]
    //public void Construct(Player player, PlayerStateMachine stateMachine)
    //{
    //    _player = player;
    //    _stateMachine = stateMachine;
    //}

    private void Start()
    {
        _player = GetComponent<Player>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        //twoRaysYDistance = topPoint.position.y - bottomPoint.position.y;
    }

    public bool IsCanClimb()
    {
        Ray bottomRay = new Ray(bottomPoint.position, bottomPoint.forward);
        Ray topRay = new Ray(topPoint.position, bottomPoint.forward);

        if (Physics.Raycast(bottomRay, out RaycastHit hit, hangRayDistance, hangRayLayerMask))
        {
            Debug.Log($"Bottom ray is catch {hit.collider.name}");            
            isOnWall = true;
            _stateMachine.SetState<OnWallState>();
        }
        else isOnWall = false;


        //if (isOnWall && !(Physics.Raycast(topRay, out RaycastHit topHit, hangRayDistance * 2, hangRayLayerMask)))
        //{
        //    Debug.Log($"PLAYER ON THE LEDGE{this}");
        //    isOnLedge = true;

        //    float fromTopOffset = 0;
        //    if (Physics.Raycast(thirdRayOrigin, Vector3.down, out RaycastHit topDownHit, twoRaysYDistance * 1.1f, hangRayLayerMask))
        //    {
        //        fromTopOffset = topDownHit.point.y - thirdRayOrigin.y;
        //        Debug.Log($"Catch cube with fromTopOffset  {fromTopOffset}");
        //    }
        //    _player.StartYAdjust(fromTopOffset, hangNormal);

        //    return true;
        //}
        //else
        //{
        //    isOnLedge = false;
        //}

        return false;
    }

    private void Update()
    {
        IsCanClimb();
        //if (isTryToHang && !isOnLedge)
        //{
        //    IsCanHang();
        //}
        //thirdRayOrigin = topPoint.position + topPoint.forward * hangRayDistance * 2f;
        Debug.DrawRay(bottomPoint.position, bottomPoint.forward * hangRayDistance, Color.blue);
        //Debug.DrawRay(topPoint.position, topPoint.forward * hangRayDistance * 2f, Color.red);
        //Debug.DrawRay(thirdRayOrigin, Vector3.down * twoRaysYDistance * 1.1f , Color.green );

    }

    private void OnWall()
    {
        Debug.Log($"Player is hanging {this}");
        _stateMachine.SetState<OnWallState>();
    }

    //public void StartTryHang()
    //{

    //    Debug.Log($"Start try hang {this}");
    //    isTryToHang = true;
    //}

    //public void StopTryHang()
    //{
    //    Debug.Log($"Stop try hang {this}");
    //    isTryToHang = false;
    //}
}
