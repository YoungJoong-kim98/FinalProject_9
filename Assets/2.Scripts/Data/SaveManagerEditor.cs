using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 그리기
        base.OnInspectorGUI();

        SaveManager manager = (SaveManager)target;

        GUILayout.Space(10);
        GUILayout.Label("=== 저장 및 불러오기 ===", EditorStyles.boldLabel);

        if (GUILayout.Button("게임 저장"))
        {
            if (TryGetPlayerComponents(out Player player, out AchievementSystem achievement))
            {
                float playTime = Time.time; // 또는 별도로 관리되는 PlayTime 변수
                manager.SaveGame(player, achievement, playTime);
            }
        }

        if (GUILayout.Button("게임 불러오기"))
        {
            if (TryGetPlayerComponents(out Player player, out AchievementSystem achievement))
            {
                float playTime = 0f;
                manager.LoadGame(player, achievement, ref playTime);
            }
        }
    }

    private bool TryGetPlayerComponents(out Player player, out AchievementSystem achievement)
    {
        player = null;
        achievement = null;

        var go = GameObject.FindGameObjectWithTag("Player");
        if (go == null)
        {
            Debug.LogWarning("플레이어 오브젝트가 존재하지 않음");
            return false;
        }

        if (!go.TryGetComponent(out player))
        {
            Debug.LogWarning("Player 컴포넌트를 찾을 수 없음");
            return false;
        }

        achievement = GameManager.Instance?.AchievementSystem;
        if (achievement == null)
        {
            Debug.LogWarning("AchievementSystem이 존재하지 않음");
            return false;
        }

        return true;
    }
}
