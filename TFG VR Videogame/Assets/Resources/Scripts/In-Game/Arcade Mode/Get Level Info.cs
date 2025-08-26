using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetLevelInfo : MonoBehaviour
{
    [HideInInspector] public LevelList allLevels;
    private string path;

    private void Awake()
    {
        path = Path.Combine(Application.streamingAssetsPath, "levels.json");
        LoadLevelData();
    }

    void LoadLevelData()
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("StreamingAssets path is empty.");
            return;
        }

        if (!File.Exists(path))
        {
            Debug.LogError($"levels.json file not found in StreamingAssets. Expected path: {path}");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            allLevels = JsonUtility.FromJson<LevelList>(json);

            if (allLevels == null || allLevels.levels == null || allLevels.levels.Count == 0)
            {
                Debug.LogError("Parsed levels.json but found no levels. Check JSON structure.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to read or parse levels.json at {path}: {ex.Message}");
        }
    }

    public Level GetLevelByNumber(int number)
    {
        if (allLevels == null || allLevels.levels == null)
        {
            Debug.LogError("Level data not loaded. Ensure levels.json is present in StreamingAssets and valid.");
            return null;
        }
        return allLevels.levels.Find(l => l.levelNumber == number);
    }
}
