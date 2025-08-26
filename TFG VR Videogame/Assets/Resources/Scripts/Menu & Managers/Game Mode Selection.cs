using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class GameModeSelection : MonoBehaviour
{
    private MySceneManager mySceneManager;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator hatAnimator;

    [SerializeField] private Animator mole2Animator;
    [SerializeField] private Animator mole2HatAnimator;

    public enum Mode
    {
        None,
        Menu,
        Arcade,
        Survival
    }

    [Header("Choose the Game Mode of this Mole:")]
    [Tooltip("Select what Game Mode is this Mole")]
    public Mode selectedMode;

    private void Awake()
    {
        animator.SetTrigger("Awake");
        hatAnimator.SetTrigger("Awake");

        GameObject sceneManagerGO = GameObject.FindWithTag("SceneManager");
        if (sceneManagerGO != null)
        {
            mySceneManager = sceneManagerGO.GetComponent<MySceneManager>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            animator.SetTrigger("Hit");
            hatAnimator.SetTrigger("Hit");

            animator.SetBool("Stunt", true);
            
            if(selectedMode != Mode.Survival)
            {
                StartCoroutine(HideAnimation());
            }
        }
    }

    IEnumerator HideAnimation()
    {
        animator.SetTrigger("Hide");
        hatAnimator.SetTrigger("Hide");

        mole2Animator.SetTrigger("Hide");
        mole2HatAnimator.SetTrigger("Hide");

        // Wait until the animator enters the "Hide" 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HideAfterHit"))
            yield return null;

        // Now wait until the animation has fully played (normalizedTime >= 1)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               || animator.IsInTransition(0))
        {
            yield return null;
        }

        // Scene Change
        if (mySceneManager != null)
        {
            switch (selectedMode)
            {
                case Mode.Menu:
                    mySceneManager.ChangeToMenu();
                    break;

                case Mode.Arcade:
                    mySceneManager.ChangeToArcade();
                    break;

                case Mode.Survival:
                    mySceneManager.ChangeToSurvival();
                    break;
            }
        }
    }
}
