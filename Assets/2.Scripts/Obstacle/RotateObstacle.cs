using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    //ȸ���ϴ� ����
    [SerializeField] private Vector3 _rotateDirection;
    //ȸ���ϴ� �ӵ�
    [SerializeField] private float _rotateSpeed = -1f;

    private void Start()
    {
        //ObstacleManager�� rotateObstacles�� �߰�
        ObstacleManager.Instance.rotateObstacles.Add(this);
 
        //������ �ʱ�ȭ
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _rotateSpeed, data.rotateSpeed);
    }

    //ȸ���ϴ� �޼���
    public void Rotate()
    {
        //����ó��
        transform.Rotate(_rotateDirection * _rotateSpeed * Time.fixedDeltaTime);
    }
}