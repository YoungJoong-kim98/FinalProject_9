using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleIdGenerator
{
    [MenuItem("Tools/Generate Obstacle IDs")]
    public static void GenerateIds()
    {
        ObstacleDataApplier[] allObstacles = GameObject.FindObjectsOfType<ObstacleDataApplier>();

        Dictionary<string, int> typeCounters = new Dictionary<string, int>();

        foreach (var obstacle in allObstacles)
        {
            string type = obstacle.obstacleType.ToString();
            if (string.IsNullOrEmpty(type))
            {
                Debug.LogWarning($"[ID Generator] {obstacle.name} has no type assigned!");
                continue;
            }

            // 번호 증가
            if (!typeCounters.ContainsKey(type))
                typeCounters[type] = 1;
            else
                typeCounters[type]++;

            string newId = $"{type}_{typeCounters[type]:D3}";

            // 유니크 ID 부여
            if (obstacle.obstacleId != newId)
            {
                Undo.RecordObject(obstacle, "Assign Obstacle ID");
                obstacle.SetId(newId);
                EditorUtility.SetDirty(obstacle);
                Debug.Log($"[ID Generator] {obstacle.name} → {newId}");
            }
        }

        Debug.Log($"[ID Generator] Complete. {allObstacles.Length} obstacles processed.");
    }
}
