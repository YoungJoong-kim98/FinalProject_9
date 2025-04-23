using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleIdGenerator
{
    [MenuItem("Tools/Generate Obstacle IDs")]
    public static void GenerateIds()
    {
        MonoBehaviour[] allBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>(true);
        List<ISaveObstacle> saveObstacles = new List<ISaveObstacle>();

        foreach (var mono in allBehaviours)
        {
            if (mono is ISaveObstacle saveObstacle)
            {
                saveObstacles.Add(saveObstacle);
            }
        }

        Dictionary<string, int> typeCounters = new Dictionary<string, int>();

        foreach (var obstacle in saveObstacles)
        {
            string type = obstacle.Type.ToString();
            if (string.IsNullOrEmpty(type))
            {
                Debug.LogWarning($"[ID Generator] Unknown obstacle type for {((Component)obstacle).name}");
                return;
            }

            // 번호 증가
            if (!typeCounters.ContainsKey(type))
                typeCounters[type] = 1;
            else
                typeCounters[type]++;

            string newId = $"{type}_{typeCounters[type]:D3}";

            // 유니크 ID 부여
            if (obstacle.Id != newId)
            {
                Undo.RecordObject((Component)obstacle, "Assign Obstacle ID");
                obstacle.SetId(newId);
                EditorUtility.SetDirty((Component)obstacle);
                Debug.Log($"[ID Generator] {((Component)obstacle).name} → {newId}");
            }
        }

        Debug.Log($"[ID Generator] Complete. {saveObstacles.Count} obstacles processed.");
    }
}
