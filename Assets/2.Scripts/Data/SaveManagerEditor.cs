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
            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null &&
                player.TryGetComponent(out Rigidbody rb) &&
                player.TryGetComponent(out AchievementSystem achievement))
            {
                float playTime = Time.time; // 필요에 따라 수정
                manager.SaveGame(player.transform, rb, achievement, playTime);
            }
            else
            {
                Debug.LogWarning("플레이어 또는 컴포넌트가 존재하지 않음");
            }
        }

        if (GUILayout.Button("게임 불러오기"))
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null &&
                player.TryGetComponent(out Rigidbody rb) &&
                player.TryGetComponent(out AchievementSystem achievement))
            {
                float playTime = 0f;
                manager.LoadGame(player.transform, rb, achievement, ref playTime);
            }
            else
            {
                Debug.LogWarning("플레이어 또는 컴포넌트가 존재하지 않음");
            }
        }
    }
}
