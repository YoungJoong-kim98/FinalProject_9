using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AchievementSystem achievementSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private NarrationManager narrationManager;
    public AchievementSystem AchievementSystem => achievementSystem;
    public SkillManager SkillManager => skillManager;
    public NarrationManager NarrationManager => narrationManager;
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
            if(skillManager == null)
            {
                GameObject go = new GameObject("SkillManager");
                skillManager = go.AddComponent<SkillManager>();
                go.transform.SetParent(this.transform);
            }
            
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
