using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mole : MonoBehaviour
{
    // --- Events --- \\
    public static Action OnHit { get; set; }

    [SerializeField] private Animator animator;
    [SerializeField] private Animator hatAnimator;
    [SerializeField] private CapsuleCollider capsuleCollider;

    // --- GO Vars --- \\
    private float timer;
    public float duration = 10f;

    private bool isActive = false;

    private void Start()
    {
        capsuleCollider.enabled = false;

        isActive = true;
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if(timer >= duration / 3)
            {
                animator.SetTrigger("Awake");
                hatAnimator.SetTrigger("Awake");

                capsuleCollider.enabled = true;
            }
            if (timer >= duration)
            {
                isActive = false;
                StartCoroutine(HideAnimation());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mace"))
        {
            isActive = false;
            OnHit?.Invoke();

            StartCoroutine(HitAndHideAnimation());
        }
    }

    IEnumerator HitAndHideAnimation()
    {
        // --- Hit and Stunt Animation --- \\
        animator.SetTrigger("Hit");
        hatAnimator.SetTrigger("Hit");

        animator.ResetTrigger("Awake");
        hatAnimator.ResetTrigger("Awake");

        // --- Hide Animation --- \\
        animator.SetTrigger("Hide");
        hatAnimator.SetTrigger("Hide");

        // Wait until the animator enters the "HideAfterHit" 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HideAfterHit"))
            yield return null;

        // Now wait until the animation has fully played (normalizedTime >= 1)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               || animator.IsInTransition(0))
        {
            yield return null;
        }

        // --- Delete GO --- \\
        DeleteGO();
    }

    IEnumerator HideAnimation()
    {
        animator.ResetTrigger("Awake");
        hatAnimator.ResetTrigger("Awake");

        // --- Hide Animation --- \\
        animator.SetTrigger("Hide");
        hatAnimator.SetTrigger("Hide");

        // Wait until the animator enters the "HideAfterHit" 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            yield return null;

        // Now wait until the animation has fully played (normalizedTime >= 1)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               || animator.IsInTransition(0))
        {
            yield return null;
        }

        // --- Delete GO --- \\
        DeleteGO();
    }

    private void DeleteGO()
    {
        Destroy(gameObject.transform.parent.parent.gameObject);
    }
}
