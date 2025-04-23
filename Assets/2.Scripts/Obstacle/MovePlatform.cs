using System;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour, ISaveObstacle
{
    //움직이는 속도
    [SerializeField] private float _moveSpeed = -1;
    //움직이는 위치
    [SerializeField] private List<Vector3> _movePositions;

    //현재 위치의 인덱스
    public int currentIndex = 0;
    public Vector3 targetPosition;
    public Vector3 startPosition;

    //플레이어 rigidbody
    private Rigidbody _playerRigidbody;

    [SerializeField] private string _id;
    [SerializeField] private ObstacleDataType _type = ObstacleDataType.MovePlatform;

    public string Id
    {
        get => _id;
        set => _id = value;
    }

    public ObstacleDataType Type => _type;

    private void Start()
    {
        //ObstacleManager의 movePlatforms에 추가
        ObstacleManager.Instance.movePlatforms.Add(this);
        AddList();

        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _moveSpeed, data.moveSpeed);

        startPosition = transform.position;
        if (_movePositions.Count > 0)
        {
            targetPosition = startPosition + _movePositions[0];
        }
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

        //현재 위치
        Vector3 oldPosition = transform.position;
        //이동
        transform.position = Vector3.MoveTowards(oldPosition, targetPosition, _moveSpeed * Time.deltaTime);

        //플레이어의 위치 처리
        if(_playerRigidbody != null)
        {
            Vector3 delta = transform.position - oldPosition;
            _playerRigidbody.MovePosition(_playerRigidbody.position + delta);
        }

        //목표 위치에 도달
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % _movePositions.Count;
            if (currentIndex == 0)
            {
                targetPosition = startPosition;
            }
            else
            {
                targetPosition = transform.position + _movePositions[currentIndex];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_movePositions == null || _movePositions.Count == 0)
            return;

        Vector3 startPos = Application.isPlaying ? startPosition : transform.position;
        Vector3 currentPos = startPos;

        Gizmos.color = Color.cyan;

        // 각 이동 위치에 Sphere 그리기 (상대좌표 누적 방식)
        foreach (Vector3 offset in _movePositions)
        {
            currentPos += offset;
            Gizmos.DrawSphere(currentPos, 0.2f);
        }

        // 선 그리기 (동선 연결)
        Gizmos.color = Color.yellow;
        currentPos = startPos;
        for (int i = 0; i < _movePositions.Count; i++)
        {
            Vector3 nextPos = currentPos + _movePositions[i];
            Gizmos.DrawLine(currentPos, nextPos);
            currentPos = nextPos;
        }

        // 루프 연결선 (마지막 위치 → 시작 위치)
        Gizmos.DrawLine(currentPos, startPos);
    }

    public void AddList()
    {
        ObstacleManager.Instance.saveObstacles.Add(this);
    }

    public void SetId(string newId)
    {
        Id = newId;
    }

    public ObstacleSaveData ToData()
    {
        ObstacleSaveData saveData = new ObstacleSaveData();
        var pos = transform.position;
        saveData.position = new float[] { pos.x, pos.y, pos.z };
        saveData.moveIndex = currentIndex;
        saveData.nextPosition = new float[] {
                        targetPosition.x,
                        targetPosition.y,
                        targetPosition.z };
        return saveData;
    }

    public void LoadtoData(ObstacleSaveData data)
    {
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        currentIndex = data.moveIndex;
        targetPosition = new Vector3(data.nextPosition[0], data.nextPosition[1], data.nextPosition[2]);
    }
}
