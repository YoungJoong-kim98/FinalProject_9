using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AchievementSystem achievementSystem;
    [SerializeField] private SkillManager skillManager;
    //[SerializeField] private NarrationManager narrationManager;
    [SerializeField] private SkillUnlockUI skillUnlockUI;
    [SerializeField] private SoundManager soundManager;
    public AchievementSystem AchievementSystem => achievementSystem;
    public SkillManager SkillManager => skillManager;
    //public NarrationManager NarrationManager => narrationManager;
    public SkillUnlockUI SkillUnlockUI => skillUnlockUI;
    public SoundManager SoundManager => soundManager;
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
            if (soundManager == null)
            {
                GameObject go = new GameObject("SoundManager");
                soundManager = go.AddComponent<SoundManager>();
                go.transform.SetParent(transform);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
