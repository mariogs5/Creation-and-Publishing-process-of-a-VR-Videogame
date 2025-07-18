using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private InputActionProperty grabAction;

    private bool isGrabActive = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        isGrabActive = false;
    }

    private void OnEnable()
    {
        Mace.onGrab += GrabMace;
        Mace.onRelease += ReleaseMace;
    }

    private void OnDisable()
    {
        isGrabActive = false;
        Mace.onGrab -= GrabMace;
        Mace.onRelease -= ReleaseMace;
    }

    private void GrabMace()
    {
        isGrabActive = true;
        animator.SetFloat("Openness", 0.62f);
    }

    private void ReleaseMace()
    {
        isGrabActive = false;
    }

    private void Update()
    {
        if (!isGrabActive)
        {
            float grabValue = grabAction.action.ReadValue<float>();
            animator.SetFloat("Openness", grabValue);
        }
    }

}
