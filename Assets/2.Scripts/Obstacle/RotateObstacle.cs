using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateDirection;
    [SerializeField] private float _rotateSpeed = -1f;

    private void Start()
    {
        ObstacleManager.Instance.rotateObstacles.Add(this);
 
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _rotateSpeed, data.rotateSpeed);
    }

    public void Rotate()
    {
        transform.Rotate(_rotateDirection * _rotateSpeed * Time.fixedDeltaTime);
    }
}