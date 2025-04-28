using System.Collections.Generic;
using UnityEngine;

public enum ObstacleDataType
{
    GlassPlatform,
    MovePlatform,
    Platform,
    PunchObstacle,
    RotateObstacle,
}

public interface IObstacle { }

public interface ISaveObstacle
{
    public string Id {  get; set; }
    public ObstacleDataType Type { get;}
    public void SetId(string newId);
    public ObstacleSaveData ToData();
    public void LoadtoData(ObstacleSaveData data);
    public void AddList();
}

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

    public List<ISaveObstacle> saveObstacles = new List<ISaveObstacle>();

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

        //_moveCoroutine = null;

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
}
