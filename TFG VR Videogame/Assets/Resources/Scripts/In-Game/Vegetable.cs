using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Vegetable : MonoBehaviour
{
    // --- Events --- \\
    public static Action OnDunk { get; set; }
    public static Action OnHit { get; set; }

    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;

    // --- GO Vars --- \\
    private float timer;
    public float duration = 10f;

    private bool isActive = false;

    private void Start()
    {
        capsuleCollider.enabled = false;

        isActive = true;

        //animator.SetTrigger("Awake");

        ////capsuleCollider.enabled = true;

        ////StartCoroutine(HideAnimation());
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer >= duration / 3)
            {
                animator.SetTrigger("Awake");

                capsuleCollider.enabled = true;
            }
            if (timer >= duration)
            {
                isActive = false;
                StartCoroutine(HideAnimation());
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        //rb.isKinematic = false;
        //rb.useGravity = true;
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

    IEnumerator HideAnimation()
    {
        animator.SetTrigger("Hide");

        animator.ResetTrigger("Awake");

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

    IEnumerator HitAndHideAnimation()
    {
        // --- Hit and Stunt Animation --- \\
        animator.SetTrigger("Hit");

        animator.ResetTrigger("Awake");

        // Wait until the animator enters the "Stunt" 
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("HitAndHide"))
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
