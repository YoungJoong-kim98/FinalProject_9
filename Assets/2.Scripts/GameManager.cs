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
                GameObject go = new GameObject("AchievementSystem"); //AchievementSystem�� GameObject ����
                achievementSystem = go.AddComponent<AchievementSystem>(); // AchievementSystem ��ũ��Ʈ �־��ֱ�
                go.transform.SetParent(this.transform); // GameManager�� �θ�� ����
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
