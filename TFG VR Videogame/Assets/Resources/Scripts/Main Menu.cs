using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameModes;

    private void OnEnable()
    {
        Mace.onGrab += StartMenu;
    }
    private void OnDisable()
    {
        Mace.onGrab -= StartMenu;
    }

    private void StartMenu()
    {

    }
}
