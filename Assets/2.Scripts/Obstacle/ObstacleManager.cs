using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    //싱글톤으로 구현
    private static ObstacleManager instance;
    public static ObstacleManager Instance { get { return instance; } }
    
    //장애물 데이터
    public ObstacleData obstacleData;

    //회전하는 장애물 리스트
    public List<RotateObstacle> rotateObstacles = new List<RotateObstacle>();
    //움직이는 발판 리스트
    public List<MovePlatform> movePlatforms = new List<MovePlatform>();

    //플레이어의 움직임 플래그 코루틴
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

        //obstacleData 초기화
        obstacleData = GetComponent<ObstacleData>();

        if (obstacleData == null)//방어코드
        {
            obstacleData = gameObject.AddComponent<ObstacleData>();
        }
    }

    private void FixedUpdate()
    {
        //회전하는 장애물
        foreach (var Rotateobstacle in rotateObstacles)
        {
            //회전하는 메서드 실행
            Rotateobstacle.Rotate();
        }

        //움직이는 발판
        foreach (var movePlatform in movePlatforms)
        {
            //움직이는 메서드 실행
            movePlatform.Move();
        }
    }

    /*
    //플레이어의 움직임 제한
    public void StartLockMovement(Player player)
    {
        //실행된 코루틴이 있으면 중지
        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        //코루틴 실행
        _moveCoroutine = StartCoroutine(SetIsMovementLocked(player));
    }

    //플레이어 움직임 제한
    private IEnumerator SetIsMovementLocked(Player player)
    {
        player.stateMachine.IsMovementLocked = true;
        yield return new WaitForSeconds(obstacleData.moveLockTime);
        player.stateMachine.IsMovementLocked = false;
        _moveCoroutine = null;
    }
    */
}
