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
                GameObject go = new GameObject("AchievementSystem"); //AchievementSystem용 GameObject 생성
                achievementSystem = go.AddComponent<AchievementSystem>(); // AchievementSystem 스크립트 넣어주기
                go.transform.SetParent(this.transform); // GameManager를 부모로 설정
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
