using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;
    public static ObstacleManager Instance { get { return instance; } }

    public ObstacleData obstacleData;

    public List<RotateObstacle> rotateObstacles = new List<RotateObstacle>();

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
        
        if(obstacleData == null)
        {
            obstacleData = gameObject.AddComponent<ObstacleData>();
        }
    }

    private void FixedUpdate()
    {
        foreach (var obstacle in rotateObstacles)
        {
            obstacle.Rotate();
        }
    }

    public void HideAndRestore(GameObject platform, float disappearTime, float appearTime)
    {
        if (!platform.TryGetComponent(out Platform go))
        { 
            return;
        }
            StartCoroutine(HideAndRestoreCoroutine(platform, disappearTime, appearTime));
    }

    private IEnumerator HideAndRestoreCoroutine(GameObject platform, float disappearTime, float appearTime)
    {
        yield return new WaitForSeconds(disappearTime);
        platform.SetActive(false);

        yield return new WaitForSeconds(appearTime);
        platform.SetActive(true);
    }
}
