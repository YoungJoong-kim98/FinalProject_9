using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AchievementSystem achievementSystem;
    public AchievementSystem AchievementSystem => achievementSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (achievementSystem == null)
            {
                GameObject go = new GameObject("AchievementSystem"); //AchievementSystem�� GameObject ����
                achievementSystem = go.AddComponent<AchievementSystem>(); // AchievementSystem ��ũ��Ʈ �־��ֱ�
                go.transform.SetParent(this.transform); // GameManager�� �θ�� ����
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
