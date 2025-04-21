using System.Collections;
using UnityEngine;

public enum PunchObstacleState
{
    None,
    Front,
    Back
}

public class PunchObstacle : MonoBehaviour
{
    //미는 힘
    [SerializeField] private float _pushPower = -1f;
    //펀치하는 속도
    [SerializeField] private float _pushSpeed = -1f;
    //돌아가는 속도
    [SerializeField] private float _backSpeed = -1f;
    //움직이는 거리
    [SerializeField] private float _moveDistance = -1f;
    //움직이는 방향
    [SerializeField] private Vector3 _direction = Vector3.forward;

    //정기적으로 실행되는지 여부
    [SerializeField] private bool _isReglar = false;
    //주기
    [SerializeField] private float _regularTime = 1f;

    //원래 위치
    private Vector3 _startPos;
    //목표 위치
    private Vector3 _targetPos;
    //플레그
    private bool _isPunching = false;
    //코루틴
    private Coroutine _punchCoroutine;

    public PunchObstacleState state = PunchObstacleState.None;

    private void Start()
    {
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _pushPower, data.pushPower);
        Utilitys.SetIfNegative(ref _pushSpeed, data.pushSpeed);
        Utilitys.SetIfNegative(ref _backSpeed, data.backSpeed);
        Utilitys.SetIfNegative(ref _moveDistance, data.moveDistance);

        //위치 초기화
        _startPos = transform.position;
        _targetPos = transform.position + _direction.normalized * _moveDistance;

        //정기적인지 여부
        if (_isReglar)
        {
            Punch();
        }
    }

    public void Init()
    {
        switch (state)
        {
            case PunchObstacleState.None:
                break;

            case PunchObstacleState.Front:
                StartCoroutine(PunchFrontMove());
                break;

            case PunchObstacleState.Back:
                StartCoroutine(PunchBackMove());
                break;

            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 충돌 중이고 실행일때
        if (collision.collider.CompareTag("Player") && _isPunching)
        {
            //미는 메서드 실행
            Push(collision.gameObject);
        }
    }

    //미는 메서드
    private void Push(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.velocity = _direction.normalized * _pushPower;
        }

        Player player = go.GetComponent<Player>();
        if (player != null)
        {
            //움직임 제한
            player.StartLockMovement(ObstacleManager.Instance.obstacleData.moveLockTime);
        }
    }

    //움직이는 메서드
    public void Punch()
    {
        //실행되는 코루틴이 없으면 코루틴 실행
        if (_punchCoroutine == null)
        {
            _punchCoroutine = StartCoroutine(PunchFrontMove());
        }
    }

    private IEnumerator PunchFrontMove()
    {
        state = PunchObstacleState.Front;

        //플래그
        _isPunching = true;

        //목표 위치로 이동
        while (Vector3.Distance(transform.position, _targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        _punchCoroutine = StartCoroutine(PunchBackMove());
    }

    private IEnumerator PunchBackMove()
    {
        state = PunchObstacleState.Back;

        _isPunching = false;

        //원래 위치로 이동
        while (Vector3.Distance(transform.position, _startPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, _backSpeed * Time.deltaTime);
            yield return null;
        }

        if (!_isReglar)
        {
            state = PunchObstacleState.None;
            _punchCoroutine = null;
            yield return null;
        }
        else
        {
            //실행 빈도 만큼 기다림
            yield return new WaitForSeconds(_regularTime);
            _punchCoroutine = StartCoroutine(PunchFrontMove());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        //방향으로 선 드로우
        Gizmos.DrawLine(transform.position, transform.position + _direction * 2f);
    }
}
