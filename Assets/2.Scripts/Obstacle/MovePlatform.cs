using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //움직이는 속도
    [SerializeField] private float _moveSpeed = -1;
    //움직이는 위치
    [SerializeField] private List<Vector3> _movePositions;

    //현재 위치의 인덱스
    private int _currentIndex = 0;

    //플레이어 rigidbody
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        //ObstacleManager의 movePlatforms에 추가
        ObstacleManager.Instance.movePlatforms.Add(this);

        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _moveSpeed, data.moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //플레이어와 충돌시
        if (collision.gameObject.CompareTag("Player"))
        {
            //_playerRigidbody 값 할당
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _playerRigidbody = rb;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //플레이어가 나갈시
        if (collision.gameObject.CompareTag("Player"))
        {
            //_playerRigidbody null 값 할당
            _playerRigidbody = null;
        }
    }

    //ObstacleManager에서 일괄적으로 실행
    public void Move()
    {
        //움직이는 위치가 없을 때 종료
        if (_movePositions == null || _movePositions.Count == 0)
            return;

        //다음 위치
        Vector3 target = _movePositions[_currentIndex];
        //현재 위치
        Vector3 oldPosition = transform.position;
        //이동
        transform.position = Vector3.MoveTowards(oldPosition, target, _moveSpeed * Time.deltaTime);

        //플레이어의 위치 처리
        if(_playerRigidbody != null)
        {
            Vector3 delta = transform.position - oldPosition;
            _playerRigidbody.MovePosition(_playerRigidbody.position + delta);
        }

        //목표 위치에 도달
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            _currentIndex = (_currentIndex + 1) % _movePositions.Count;
        }
    }

    private void OnDrawGizmos()
    {
        //움직이는 위치가 없을 때 종료
        if (_movePositions == null || _movePositions.Count == 0)
            return;

        //움직이는 위치에 구 드로우
        Gizmos.color = Color.cyan;
        foreach (Vector3 pos in _movePositions)
        {
            Gizmos.DrawSphere(pos, 0.2f);
        }

        //움직이는 동선에 선 드로우
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _movePositions.Count - 1; i++)
        {
            Gizmos.DrawLine(_movePositions[i], _movePositions[i + 1]);
        }

        Gizmos.DrawLine(_movePositions[_movePositions.Count - 1], _movePositions[0]);
    }
}
