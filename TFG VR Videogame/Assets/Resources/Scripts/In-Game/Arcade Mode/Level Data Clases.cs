using System.Collections.Generic;

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
