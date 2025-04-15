using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    //회전하는 방향
    [SerializeField] private Vector3 _rotateDirection;
    //회전하는 속도
    [SerializeField] private float _rotateSpeed = -1f;

    private void Start()
    {
        //ObstacleManager의 rotateObstacles에 추가
        ObstacleManager.Instance.rotateObstacles.Add(this);
 
        //데이터 초기화
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _rotateSpeed, data.rotateSpeed);
    }

    //회전하는 메서드
    public void Rotate()
    {
        //물리처리
        transform.Rotate(_rotateDirection * _rotateSpeed * Time.fixedDeltaTime);
    }
}