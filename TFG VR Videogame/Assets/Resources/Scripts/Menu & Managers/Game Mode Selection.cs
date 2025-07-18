using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class GameModeSelection : MonoBehaviour
{
    private MySceneManager mySceneManager;

    public enum Mode
    {
        None,
        Arcade,
        Survival
    }

    [Header("Choose the Game Mode of this Mole:")]
    [Tooltip("Select what Game Mode is this Mole")]
    public Mode selectedMode;

    private void Awake()
    {
        // Scene Manager
        GameObject sceneManagerGO = GameObject.FindWithTag("SceneManager");
        if (sceneManagerGO != null)
        {
            mySceneManager = sceneManagerGO.GetComponent<MySceneManager>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("AA");

        if (collision.gameObject.CompareTag("Mace"))
        {
            Debug.Log("Mole Hitted");
            if(mySceneManager != null)
            {
                if(selectedMode == Mode.Arcade)
                {
                    mySceneManager.ChangeToArcade();
                }
                else if (selectedMode == Mode.Survival)
                {
                    mySceneManager.ChangeToSurvival();
                }
            }
            Destroy(gameObject);
        }
    }
}
