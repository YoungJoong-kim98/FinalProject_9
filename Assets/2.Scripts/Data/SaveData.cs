using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public float[] playerPosition;
    public float[] playerVelocity;
    public float playTime;
    public float movelockRemainTime;
    //스킬획득 여부
    public bool run;
    public bool doubleJump;
    public bool grab;

    //나레이션 인덱스

    //업적 정보
    public AchievementData achievement;
}

[System.Serializable]
public class AchievementData
{
    public int jumpCount;
    public int jumpPlatform;
    public bool researcherStage;
    public bool fallingCrash;
    public int grabCount;
    public bool completionTime;
}

[System.Serializable]
public class ObstacleSaveEntry
{
    public string id;
    public ObstacleSaveData data;

    public ObstacleSaveEntry(string id, ObstacleSaveData data)
    {
        this.id = id;
        this.data = data;
    }
}

[System.Serializable]
public class ObstacleSaveWrapper
{
    public List<ObstacleSaveEntry> obstacleList = new();

    public void FromDictionary(Dictionary<string, ObstacleSaveData> dict)
    {
        obstacleList.Clear();
        foreach (var kvp in dict)
        {
            obstacleList.Add(new ObstacleSaveEntry(kvp.Key, kvp.Value));
        }
    }

    public Dictionary<string, ObstacleSaveData> ToDictionary()
    {
        var dict = new Dictionary<string, ObstacleSaveData>();
        foreach (var entry in obstacleList)
        {
            dict[entry.id] = entry.data;
        }
        return dict;
    }
}

[System.Serializable]
public class ObstacleSaveData
{
    public ObstacleDataType type;

    public bool isActive;
    public float[] position;
    public float remainTime;

    public GlassPlatformState glassPlatformState;
    //
    public PlatformState platformState;
    //
    public PunchObstacleState punchObstacleState;
    //
    public int moveIndex;
    //
    public float[] rotation;
}
