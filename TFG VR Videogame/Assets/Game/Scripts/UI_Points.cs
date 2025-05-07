using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UI_Points : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshPro text_Score;

    private void OnEnable()
    {
        NewMole.OnHit += UpdateScore;
        NewVegetable.OnDunk += UpdateScore;
    }

    private void UpdateScore()
    {
        ++score;
        text_Score.text = "Score: " + score;
    }
}
