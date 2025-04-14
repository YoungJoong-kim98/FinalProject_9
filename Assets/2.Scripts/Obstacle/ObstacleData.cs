using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    [Header("Platform")]
    public float disapearTime = 1;
    public float apearTime = 5;

    [Header("Jump Platform")]
    public float jumpPower = 15;

    [Header("Punch Obstacle")]
    public float pushPower = 15f;
    public float pushSpeed = 15f;
    public float backSpeed = 5f;
    public float moveDistance = 5f;

    [Header("Reflect Obstacle")]
    public float reflectPower = 30f;
    public float reflectMinPower = 15f;
    public float reflectMaxPower = 60f;
    public float reflectTime = 0.5f;

    [Header("Rotate Obstacle")]
    public float rotateSpeed = 10f;

    [Header("Fan Area")]
    public float forceStrength = 1f;

    [Header("Move Platform")]
    public float moveSpeed = 5f;
}
