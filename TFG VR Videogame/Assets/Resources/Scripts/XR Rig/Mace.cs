using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mace : MonoBehaviour
{
    public static Action onGrab { get; set; }
    public static Action onRelease { get; set; }

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

    private void OnGrab(SelectEnterEventArgs args)
    {
        onGrab?.Invoke();

        //if (!isGrabed)
        //{
        //    onGrab?.Invoke();

        //    isGrabed = true;
        //    rb.useGravity = true;
        //    rb.isKinematic = false;
        //}
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        onRelease?.Invoke();

        //if (isGrabed)
        //{
        //    isGrabed = false;
        //    onRelease?.Invoke();
        //}
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
