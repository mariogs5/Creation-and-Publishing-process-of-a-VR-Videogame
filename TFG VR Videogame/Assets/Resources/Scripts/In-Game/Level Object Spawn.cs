using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectSpawn : MonoBehaviour
{
    [SerializeField] private GetLevelInfo levelInfoScript;
    [SerializeField] private GameObject center;

    public int currentLevel;
    private Level currentLevelInfo;

    private Vector3 spawn_1 , spawn_2, spawn_3, spawn_4, spawn_5, spawn_6, spawn_7;
    private void Awake()
    {
        spawn_1 = new Vector3(-1.014f, 0.608207703f, -0.771000028f);

        spawn_2 = new Vector3(-0.505999982f, 0.608207703f, -0.152999997f);
        spawn_3 = new Vector3(-0.270000011f, 0.608207703f, 0.236000001f);
        spawn_4 = new Vector3(0, 0.608207703f, -0.152999997f);
        spawn_5 = new Vector3(0.270000011f, 0.608207703f, 0.236000001f);
        spawn_6 = new Vector3(0.505999982f, 0.608207703f, -0.152999997f);

        spawn_7 = new Vector3(1.014f, 0.608207703f, -0.771000028f);
    }

    public void StartLevel()
    {
        currentLevelInfo = levelInfoScript.GetLevelByNumber(currentLevel);


    }
}