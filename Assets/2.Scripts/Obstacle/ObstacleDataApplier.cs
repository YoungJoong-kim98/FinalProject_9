using UnityEngine;

public enum ObstacleDataType
{
    GlassPlatform,
    MovePlatform,
    Platform,
    PunchObstacle,
    RotateObstace
}

public class ObstacleDataApplier : MonoBehaviour
{
    [SerializeField] private string _obstacleId;
    [SerializeField] private ObstacleDataType _obstacleType;

    public string obstacleId => _obstacleId;
    public ObstacleDataType obstacleType => _obstacleType;

    public ObstacleSaveData CreateSaveData()
    {
        var saveData = new ObstacleSaveData();
        saveData.type = _obstacleType;

        switch (obstacleType)
        {
            case ObstacleDataType.GlassPlatform:
                if(TryGetComponent(out GlassPlatform glassPlatform))
                {
                    saveData.glassPlatformState = glassPlatform.state;
                }
                break;

            case ObstacleDataType.MovePlatform:
                if (TryGetComponent(out MovePlatform movePlatform))
                {
                    var pos = transform.position;
                    saveData.position = new float[] { pos.x, pos.y, pos.z };
                    saveData.moveIndex = movePlatform.currentIndex;
                }
                break;

            case ObstacleDataType.Platform:
                if(TryGetComponent(out Platform platform))
                {
                    saveData.remainTime = platform.remainTime;
                    saveData.platformState = platform.state;
                }
                break;

            case ObstacleDataType.PunchObstacle:
                if (TryGetComponent(out PunchObstacle punchObstacle))
                {
                    var pos = punchObstacle.transform.position;
                    saveData.position = new float[3] { pos.x, pos.y, pos.z };
                    saveData.punchObstacleState = punchObstacle.state;
                }
                break;

            case ObstacleDataType.RotateObstace:
                if(TryGetComponent(out RotateObstacle rotateObstace))
                {
                    var angle = rotateObstace.transform.eulerAngles;
                    saveData.rotation = new float[3] { angle.x, angle.y, angle.z };
                }
                break;

            default:
                break;
        }

        return saveData;
    }

#if UNITY_EDITOR
    public void SetId(string newId)
    {
        _obstacleId = newId;
    }
#endif
}
