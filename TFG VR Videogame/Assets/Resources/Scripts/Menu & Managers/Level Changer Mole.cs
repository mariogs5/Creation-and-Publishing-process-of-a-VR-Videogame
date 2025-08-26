using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelChangerMole : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator hatAnimator;

    [SerializeField] private LevelObjectSpawn objectSpawnScript;
    [SerializeField] private SaveFileManager saveFile;

    [SerializeField] private TextMeshPro lvlNumberUI;
    [SerializeField] private TextMeshPro lvlScoreUI;

    [SerializeField] private GameObject parentGO;

    [SerializeField] private Animator mole1Animator;
    [SerializeField] private Animator mole1HatAnimator;

    [SerializeField] private Animator mole2Animator;
    [SerializeField] private Animator mole2HatAnimator;

    public enum Type
    {
        None,
        Previous,
        Next,
        Start
    }

    public Type moleType;

    private bool ishit = false;

    private MySceneManager mySceneManager;

    private void Awake()
    {
        ishit = false;

        animator.SetTrigger("Awake");
        hatAnimator.SetTrigger("Awake");

        GameObject sceneManagerGO = GameObject.FindWithTag("SceneManager");
        if (sceneManagerGO != null)
        {
            mySceneManager = sceneManagerGO.GetComponent<MySceneManager>();

            ChangeUIScore();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            if (!ishit)
            {
                animator.SetTrigger("Hit");
                hatAnimator.SetTrigger("Hit");
                animator.SetBool("Stunt", true);
            }
            else
            {
                animator.SetTrigger("Hit");
            }

            switch (moleType)
            {
                case Type.None:
                    break;

                case Type.Previous:
                    if(mySceneManager.levelNumber > 1)
                    {
                        mySceneManager.levelNumber -= 1;
                    }
                    break;

                case Type.Next:
                    if(mySceneManager.levelNumber < 30 && mySceneManager.levelNumber < saveFile.GetMaxLevelReached())
                    {
                        mySceneManager.levelNumber += 1;
                    }
                    break;

                case Type.Start:
                    if(mySceneManager.levelNumber > 0 && mySceneManager.levelNumber <= 30)
                    {
                        StartCoroutine(HideAnimationAndStartLevel());
                    }
                    break;
            }

            ChangeUIScore();
        }
    }

    private void ChangeUIScore()
    {
        lvlNumberUI.text = "Level " + mySceneManager.levelNumber.ToString();

        if(saveFile.GetLevelScore(mySceneManager.levelNumber) == -1)
        {
            lvlScoreUI.text = "Play the level to get a score";
        }
        else
        {
            lvlScoreUI.text = "Score: " + saveFile.GetLevelScore(mySceneManager.levelNumber).ToString();
        }
    }

    IEnumerator HideAnimationAndStartLevel()
    {
        animator.SetTrigger("Hide");
        hatAnimator.SetTrigger("Hide");

        mole1Animator.SetTrigger("Hide");
        mole1HatAnimator.SetTrigger("Hide");

        mole2Animator.SetTrigger("Hide");
        mole2HatAnimator.SetTrigger("Hide");

        //yield return null;

        // Wait until the animator enters the "Hide" 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HideAfterHit"))
            yield return null;

        // Now wait until the animation has fully played (normalizedTime >= 1)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               || animator.IsInTransition(0))
        {
            yield return null;
        }

        if (objectSpawnScript != null) objectSpawnScript.StartLevel(mySceneManager.levelNumber);
        parentGO.SetActive(false);
    }
}
