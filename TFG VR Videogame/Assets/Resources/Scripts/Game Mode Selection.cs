using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject sceneManagerGO = GameObject.FindWithTag("SceneManager");
        if (sceneManagerGO != null)
        {
            mySceneManager = sceneManagerGO.GetComponent<MySceneManager>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mace")
        {
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
