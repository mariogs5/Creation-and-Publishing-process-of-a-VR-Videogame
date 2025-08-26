using System;
using System.Collections.Generic;

// ------- Level Layout Classes ------- \\
[System.Serializable]
public class SpawnData
{
    public int spotIndex;
    public bool isMole;
}

[System.Serializable]
public class Level
{
    public int levelNumber;
    public List<SpawnData> spawns;
    public float spawnSpeed;
    public float lifeTime;
}

[System.Serializable]
public class LevelList
{
    public List<Level> levels;
}

// ------- Save File Classes ------- \\
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
