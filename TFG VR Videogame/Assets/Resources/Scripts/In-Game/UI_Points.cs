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
        Vegetable.OnDunk += VegUpdateScore;
    }

    private void OnDisable()
    {
        Mole.OnHit -= MoleUpdateScore;
        Vegetable.OnDunk -= VegUpdateScore;
    }

    private void MoleUpdateScore()
    {
        objectSpawn.score += 500;
        scoreUI.text = "Score: " + objectSpawn.score;
    }

    private void VegUpdateScore()
    {

    }
}
