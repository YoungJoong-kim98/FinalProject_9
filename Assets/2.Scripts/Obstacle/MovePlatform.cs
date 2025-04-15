using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //�����̴� �ӵ�
    [SerializeField] private float _moveSpeed = -1;
    //�����̴� ��ġ
    [SerializeField] private List<Vector3> _movePositions;

    //���� ��ġ�� �ε���
    private int _currentIndex = 0;

    //�÷��̾� rigidbody
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        //ObstacleManager�� movePlatforms�� �߰�
        ObstacleManager.Instance.movePlatforms.Add(this);

        //������ �ʱ�ȭ
        var data = ObstacleManager.Instance.obstacleData;
        Utilitys.SetIfNegative(ref _moveSpeed, data.moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾�� �浹��
        if (collision.gameObject.CompareTag("Player"))
        {
            //_playerRigidbody �� �Ҵ�
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _playerRigidbody = rb;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //�÷��̾ ������
        if (collision.gameObject.CompareTag("Player"))
        {
            //_playerRigidbody null �� �Ҵ�
            _playerRigidbody = null;
        }
    }

    //ObstacleManager���� �ϰ������� ����
    public void Move()
    {
        //�����̴� ��ġ�� ���� �� ����
        if (_movePositions == null || _movePositions.Count == 0)
            return;

        //���� ��ġ
        Vector3 target = _movePositions[_currentIndex];
        //���� ��ġ
        Vector3 oldPosition = transform.position;
        //�̵�
        transform.position = Vector3.MoveTowards(oldPosition, target, _moveSpeed * Time.deltaTime);

        //�÷��̾��� ��ġ ó��
        if(_playerRigidbody != null)
        {
            Vector3 delta = transform.position - oldPosition;
            _playerRigidbody.MovePosition(_playerRigidbody.position + delta);
        }

        //��ǥ ��ġ�� ����
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            _currentIndex = (_currentIndex + 1) % _movePositions.Count;
        }
    }

    private void OnDrawGizmos()
    {
        //�����̴� ��ġ�� ���� �� ����
        if (_movePositions == null || _movePositions.Count == 0)
            return;

        //�����̴� ��ġ�� �� ��ο�
        Gizmos.color = Color.cyan;
        foreach (Vector3 pos in _movePositions)
        {
            Gizmos.DrawSphere(pos, 0.2f);
        }

        //�����̴� ������ �� ��ο�
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _movePositions.Count - 1; i++)
        {
            Gizmos.DrawLine(_movePositions[i], _movePositions[i + 1]);
        }

        Gizmos.DrawLine(_movePositions[_movePositions.Count - 1], _movePositions[0]);
    }
}
