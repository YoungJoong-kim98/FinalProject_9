using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public static SaveManager Instance {get {return instance;}}

    private string _savePlayerPath => Path.Combine(Application.persistentDataPath, "PlayerSave.json");
    private string _saveObstaclePath => Path.Combine(Application.persistentDataPath, "ObstacleSave.json");

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(Transform playerTransform, Rigidbody rigidbody ,AchievementSystem achievementSystem, float playTime)
    {
        Debug.Log(Application.persistentDataPath);
        SavePlayerData(playerTransform, rigidbody, achievementSystem, playTime);
        SaveObstacleStates();
    }

    public void SavePlayerData(Transform playerTransform, Rigidbody rigidbody, AchievementSystem achievementSystem, float playTime)
    {
        PlayerSaveData PlayerData = new PlayerSaveData()
        {
            playerPosition = new float[3]
            {
                playerTransform.position.x,
                playerTransform.position.y,
                playerTransform.position.z,
            },
            playerVelocity = new float[3]
            {
                rigidbody.velocity.x,
                rigidbody.velocity.y,
                rigidbody.velocity.z,
            },
            playTime = playTime,
            achievement = achievementSystem.ToData()
        };

        string json = JsonUtility.ToJson(PlayerData, true);
        File.WriteAllText(_savePlayerPath, json);
        Debug.Log("Game Saved to: " + _savePlayerPath);
    }

    public void SaveObstacleStates()
    {
        //var dataWrapper = new ObstacleSaveWrapper();
        //foreach (var applier in FindObjectsOfType<ObstacleDataApplier>())
        //{
        //    dataWrapper.obstacles[applier.obstacleId] = applier.CreateSaveData();
        //}

        //string json = JsonUtility.ToJson(dataWrapper.obstacles, true);
        //Debug.Log(json);
        //File.WriteAllText(_saveObstaclePath, json);
        var dict = new Dictionary<string, ObstacleSaveData>();

        foreach (var applier in FindObjectsOfType<ObstacleDataApplier>())
        {
            dict[applier.obstacleId] = applier.CreateSaveData();
        }

        var dataWrapper = new ObstacleSaveWrapper();
        dataWrapper.FromDictionary(dict); // List 형태로 변환

        string json = JsonUtility.ToJson(dataWrapper, true);
        Debug.Log(json);
        File.WriteAllText(_saveObstaclePath, json);
    }

    public void LoadGame(Transform playerTransform, Rigidbody rigidbody, AchievementSystem achievementSystem, ref float playTime)
    {
        LoadPlayerData(playerTransform,rigidbody, achievementSystem,ref playTime);
        LoadAllObstacleState();
    }

    public void LoadPlayerData(Transform playerTransform, Rigidbody rigidbody, AchievementSystem achievementSystem, ref float playTime)
    {
        if (File.Exists(_savePlayerPath))
        {
            string json = File.ReadAllText(_savePlayerPath);
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

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

            Vector3 vel = new Vector3(
                data.playerVelocity[0],
                data.playerVelocity[1],
                data.playerVelocity[2]
            );

            playerTransform.position = pos;
            rigidbody.velocity = vel;
            achievementSystem.LoadFromData(data.achievement);
            playTime = data.playTime;
            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }

    public void LoadAllObstacleState()
    {
        if (!File.Exists(_saveObstaclePath))
        {
            Debug.LogWarning("Obstacle save file not found.");
            return;
        }

        string json = File.ReadAllText(_saveObstaclePath);
        var wrapper = JsonUtility.FromJson<ObstacleSaveWrapper>(json);

        var loadedData = wrapper.ToDictionary();

        foreach (var obstacle in FindObjectsOfType<ObstacleDataApplier>())
        {
            var id = obstacle.obstacleId;
            //if (!wrapper.obstacles.TryGetValue(id, out var data)) continue;
            if (!loadedData.TryGetValue(id, out var data)) continue;
            switch (data.type)
            {
                case ObstacleDataType.GlassPlatform:
                    if (obstacle.TryGetComponent(out GlassPlatform glassPlatform))
                    {
                        glassPlatform.state = data.glassPlatformState;
                        glassPlatform.Init();
                    }
                    break;

                case ObstacleDataType.MovePlatform:
                    if (obstacle.TryGetComponent(out MovePlatform movePlatform))
                    {
                        movePlatform.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                        movePlatform.currentIndex = data.moveIndex;
                        Debug.Log($"moveIndex : {data.moveIndex}");
                    }
                    break;

                case ObstacleDataType.Platform:
                    if (obstacle.TryGetComponent(out Platform platform))
                    {
                        platform.state = data.platformState;
                        platform.remainTime = data.remainTime;
                        platform.Init();
                    }
                    break;

                case ObstacleDataType.PunchObstacle:
                    if (obstacle.TryGetComponent(out PunchObstacle punchObstacle))
                    {
                        punchObstacle.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                        punchObstacle.state = data.punchObstacleState;
                        punchObstacle.Init();
                    }
                    break;

                case ObstacleDataType.RotateObstace:
                    if (obstacle.TryGetComponent(out RotateObstacle rotateObstacle))
                    {
                        rotateObstacle.transform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
