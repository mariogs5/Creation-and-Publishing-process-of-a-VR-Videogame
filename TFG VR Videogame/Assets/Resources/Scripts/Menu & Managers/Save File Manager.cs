using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class LevelSave
{
    public int levelID;
    public int score;
}

[Serializable]
public class SaveData
{
    public List<LevelSave> levels = new List<LevelSave>();
}

public class SaveFileManager : MonoBehaviour
{
    private string saveFilePath;
    private SaveData saveData;

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        LoadAll();
    }

    public void LoadAll()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
                Debug.Log("Loaded save file with " + saveData.levels.Count + " entries.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed reading save file: " + ex.Message);
                saveData = new SaveData();
            }
        }
        else
        {
            Debug.Log("No save file found—starting fresh.");
            saveData = new SaveData();
            // Create empty file if needed
        }
    }

    public void SaveLevel(int levelID, int score)
    {
        if (saveData == null) saveData = new SaveData();

        var entry = saveData.levels.Find(e => e.levelID == levelID);
        if (entry == null)
        {
            saveData.levels.Add(new LevelSave { levelID = levelID, score = score });
        }
        else if (score > entry.score)
        {
            entry.score = score;
        }
        else
        {
            Debug.Log($"Not updating: existing score {entry.score} is higher than {score}");
        }

        string json = JsonUtility.ToJson(saveData, true);
        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Save file written/updated at: " + saveFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to write save file: " + ex.Message);
        }
    }

    public int GetLevelScore(int levelID)
    {
        if (saveData == null) return -1;
        var entry = saveData.levels.Find(e => e.levelID == levelID);
        return entry != null ? entry.score : -1;
    }
}
