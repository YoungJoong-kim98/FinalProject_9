using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string _savePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void SaveGame(Transform playerTransform, AchievementSystem achievementSystem, float playTime)
    {
        SaveData data = new SaveData()
        {
            playerPosition = new float[3]
            {
                playerTransform.position.x,
                playerTransform.position.y,
                playerTransform.position.z
            },
            achievement = achievementSystem.ToData(),
            playTime = playTime
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
        Debug.Log("Game Saved to: " + _savePath);
    }

    public void LoadGame(Transform playerTransform, AchievementSystem achievementSystem, ref float playTime)
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
            {
                Debug.LogWarning("SaveData is crushed");
                return;
            }

            Vector3 pos = new Vector3(
                data.playerPosition[0],
                data.playerPosition[1],
                data.playerPosition[2]
            );

            playerTransform.position = pos;
            achievementSystem.LoadFromData(data.achievement);
            playTime = data.playTime;
            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }
}