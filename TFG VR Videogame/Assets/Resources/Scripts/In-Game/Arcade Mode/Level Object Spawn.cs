using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelObjectSpawn : MonoBehaviour
{
    public int currentLevel;
    private Level currentLevelInfo;
    [SerializeField] private GetLevelInfo levelInfoScript;
    [SerializeField] private SaveFileManager saveFile;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject goBackMoles;

    private Vector3[] spawnPositions;
    [SerializeField] private GameObject SpawnCenterGO;

    [SerializeField] private TextMeshPro countdownTextUI;
    [SerializeField] private TextMeshPro scoreUI;
    [HideInInspector] public int score;

    private void Awake()
    {
        spawnPositions = new Vector3[]
        {
            new Vector3(-0.507f, 0.3041039f, -0.3855f), // spot 0
            new Vector3(-0.253f, 0.3041039f, -0.0765f), // spot 1
            new Vector3(-0.135f, 0.3041039f,  0.118f), // spot 2
            new Vector3( 0.0000f, 0.3041039f, -0.0765f), // spot 3
            new Vector3( 0.135f, 0.3041039f,  0.118f), // spot 4
            new Vector3( 0.253f, 0.3041039f, -0.0765f), // spot 5
            new Vector3( 0.507f, 0.3041039f, -0.3855f)  // spot 6
        };
    }

    public void StartLevel(int level)
    {
        currentLevel = level;
        currentLevelInfo = levelInfoScript.GetLevelByNumber(currentLevel);

        if (currentLevelInfo == null)
        {
            Debug.LogError($"Level {currentLevel} not found.");
            return;
        }

        StartCoroutine(LevelRoutine());
    }

    private IEnumerator LevelRoutine()
    {
        float countdown = 3f;
        while (countdown > 0f)
        {
            if (countdownTextUI != null)
            {
                countdownTextUI.text = Mathf.Ceil(countdown).ToString();
            }
            else
            {
                Debug.Log($"Game starts in {Mathf.Ceil(countdown)}...");
            }

            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        countdownTextUI.text = "";
        scoreUI.text = "Score: 0";

        foreach (var spawnData in currentLevelInfo.spawns)
        {
            yield return new WaitForSeconds(currentLevelInfo.spawnSpeed);

            int idx = spawnData.spotIndex;
            if (idx < 0 || idx >= spawnPositions.Length)
            {
                Debug.LogWarning($"SpotIndex {idx} out of range.");
                continue;
            }

            Vector3 pos = spawnPositions[idx];

            GameObject obj = Instantiate(slotPrefab);

            Slot slotScript = obj.GetComponent<Slot>();
            slotScript.isMole = spawnData.isMole;
            slotScript.lifeTime = currentLevelInfo.lifeTime;

            obj.transform.SetParent(gameObject.transform, false);
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
        }
        StartCoroutine(LevelFinish());
    }

    private IEnumerator LevelFinish()
    {
        yield return new WaitForSeconds(currentLevelInfo.lifeTime + 3f);

        float countdown = 3f;
        while (countdown > 0f)
        {
            if (scoreUI != null)
            {
                scoreUI.text = "Level Finished";
            }

            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        scoreUI.text = scoreUI.text + "\n Score: " + score.ToString();
        saveFile.SaveLevel(currentLevel, score);
        goBackMoles.SetActive(true);
    }
}