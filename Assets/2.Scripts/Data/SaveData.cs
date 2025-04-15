[System.Serializable]
public class SaveData
{
    public float[] playerPosition;
    public float playTime;

    //��ųȹ�� ����

    //�����̼� �ε���

    //���� ����
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
