using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetLevelInfo : MonoBehaviour
{
    public LevelList allLevels;
    public Level currentLevel;

    void Start()
    {
        LoadLevelData();
    }

    void LoadLevelData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "levels.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allLevels = JsonUtility.FromJson<LevelList>(json);
        }
        else
        {
            Debug.LogError("levels.json file not found in StreamingAssets.");
        }
    }

    public Level GetLevelByNumber(int number)
    {
        return allLevels.levels.Find(l => l.levelNumber == number);
    }
}
