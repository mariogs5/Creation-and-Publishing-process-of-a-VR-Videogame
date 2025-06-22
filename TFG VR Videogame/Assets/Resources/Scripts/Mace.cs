using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mace : MonoBehaviour
{
    private StickyGrabInteractable grabInteractable;
    private Rigidbody rb;
    public bool isGrabed = false;

    private void Awake()
    {
        isGrabed = false;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<StickyGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!isGrabed)
        {
            isGrabed = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isGrabed)
        {
            isGrabed = false;
        }
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
