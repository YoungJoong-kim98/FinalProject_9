using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;
    public static ObstacleManager Instance { get { return instance; } }

    public ObstacleData obstacleData;

    public List<RotateObstacle> rotateObstacles = new List<RotateObstacle>();

    public List<MovePlatform> movePlatforms = new List<MovePlatform>();

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

        obstacleData = GetComponent<ObstacleData>();

        if (obstacleData == null)
        {
            obstacleData = gameObject.AddComponent<ObstacleData>();
        }
    }

    private void FixedUpdate()
    {
        foreach (var Rotateobstacle in rotateObstacles)
        {
            Rotateobstacle.Rotate();
        }

        foreach (var movePlatform in movePlatforms)
        {
            movePlatform.Move();
        }
    }
}
