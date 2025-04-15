using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //�̱������� ����
    private static ObstacleManager instance;
    public static ObstacleManager Instance { get { return instance; } }
    
    //��ֹ� ������
    public ObstacleData obstacleData;

    //ȸ���ϴ� ��ֹ� ����Ʈ
    public List<RotateObstacle> rotateObstacles = new List<RotateObstacle>();
    //�����̴� ���� ����Ʈ
    public List<MovePlatform> movePlatforms = new List<MovePlatform>();

    //�÷��̾��� ������ �÷��� �ڷ�ƾ
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _moveCoroutine = null;

        //obstacleData �ʱ�ȭ
        obstacleData = GetComponent<ObstacleData>();

        if (obstacleData == null)//����ڵ�
        {
            obstacleData = gameObject.AddComponent<ObstacleData>();
        }
    }

    private void FixedUpdate()
    {
        //ȸ���ϴ� ��ֹ�
        foreach (var Rotateobstacle in rotateObstacles)
        {
            //ȸ���ϴ� �޼��� ����
            Rotateobstacle.Rotate();
        }

        //�����̴� ����
        foreach (var movePlatform in movePlatforms)
        {
            //�����̴� �޼��� ����
            movePlatform.Move();
        }
    }

    //�÷��̾��� ������ ����
    public void StartLockMovement(Player player)
    {
        //����� �ڷ�ƾ�� ������ ����
        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        //�ڷ�ƾ ����
        _moveCoroutine = StartCoroutine(SetIsMovementLocked(player));
    }

    //�÷��̾� ������ ����
    private IEnumerator SetIsMovementLocked(Player player)
    {
        player.stateMachine.IsMovementLocked = true;
        yield return new WaitForSeconds(obstacleData.moveLockTime);
        player.stateMachine.IsMovementLocked = false;
        _moveCoroutine = null;
    }
}
