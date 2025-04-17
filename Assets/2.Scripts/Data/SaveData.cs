using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public float[] playerPosition;
    public float[] playerVelocity;
    public float playTime;
    //스킬획득 여부

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
public class ObstacleSaveWrapper
{
    public Dictionary<string, ObstacleSaveData> obstacles = new();
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
}
