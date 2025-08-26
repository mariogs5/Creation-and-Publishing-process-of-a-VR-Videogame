using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Points : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreUI;
    [SerializeField] private LevelObjectSpawn objectSpawn;

    private void OnEnable()
    {
        Mole.OnHit += MoleUpdateScore;
        Vegetable.OnDunk += VegDunkUpdateScore;
        Vegetable.OnHit += VegHitUpdateScore;
    }

    private void OnDisable()
    {
        Mole.OnHit -= MoleUpdateScore;
        Vegetable.OnDunk -= VegDunkUpdateScore;
        Vegetable.OnHit -= VegHitUpdateScore;
    }

    private void MoleUpdateScore()
    {
        objectSpawn.score += 500;
        scoreUI.text = "Score: " + objectSpawn.score;
    }

    private void VegHitUpdateScore()
    {
        if(objectSpawn.score > 100)
        {
            objectSpawn.score -= 100;
            scoreUI.text = "Score: " + objectSpawn.score;
        }
    }

    private void VegDunkUpdateScore()
    {

    }
}
