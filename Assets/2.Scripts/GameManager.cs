using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AchievementSystem achievementSystem;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private NarrationManager narrationManager;
    [SerializeField] private SkillUnlockUI skillUnlockUI;
    public AchievementSystem AchievementSystem => achievementSystem; //업적 시스템
    public SkillManager SkillManager => skillManager; //스킬 매니저
    public NarrationManager NarrationManager => narrationManager; //나레이션
    public SkillUnlockUI SkillUnlockUI => skillUnlockUI; //스킬 UI 추후 UI매니저로 이동
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
            if (narrationManager == null)
            {
                var prefab = Resources.Load<NarrationManager>("Narration/NarrationManager");
                if (prefab != null)
                {
                    narrationManager = Instantiate(prefab, transform);
                }
                else
                {
                    Debug.LogError("NarrationManager 프리팹을 Resources에서 찾을 수 없습니다.");
                }
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }


}
