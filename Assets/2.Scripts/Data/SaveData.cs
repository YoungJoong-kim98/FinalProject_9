[System.Serializable]
public class SaveData
{
    public float[] playerPosition;
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
