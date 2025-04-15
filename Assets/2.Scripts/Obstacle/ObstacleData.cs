using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    //플레이어의 관련
    [Header("Player")]
    //플레이의 움직임이 제한되는 시간
    public float moveLockTime = 0.5f;

    //움직이는 발판
    [Header("Platform")]
    //사라지기까지의 시간
    public float disapearTime = 1;
    //생성되기까지의 시간
    public float apearTime = 5;

    //점프 발판
    [Header("Jump Platform")]
    //점프하는 힘
    public float jumpPower = 15;

    //펀치 장애물
    [Header("Punch Obstacle")]
    //미는 힘
    public float pushPower = 15f;
    //펀치하는 속도
    public float pushSpeed = 15f;
    //돌아가는 속도
    public float backSpeed = 5f;
    //움직이는 거리
    public float moveDistance = 5f;

    //반사 장애물
    [Header("Reflect Obstacle")]
    //반사하는 힘
    public float reflectPower = 30f;
    //최소 힘
    public float reflectMinPower = 15f;
    //최대 힘
    public float reflectMaxPower = 60f;

    //회전하는 장애물
    [Header("Rotate Obstacle")]
    //회전하는 속도
    public float rotateSpeed = 10f;

    //선풍기 구역
    [Header("Fan Area")]
    //선풍기 힘
    public float forceStrength = 1f;

    //움직이는 발판
    [Header("Move Platform")]
    //움직이는 속도
    public float moveSpeed = 5f;
}
