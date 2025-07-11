using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mace : MonoBehaviour
{
    public static Action onGrab { get; set; }

    //private StickyGrabInteractable grabInteractable;
    private XRGrabInteractable grabInteractable2;

    private Rigidbody rb;
    public bool isGrabed = false;

    public bool testGrab;

    private void Awake()
    {
        isGrabed = false;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody>();
        //grabInteractable = GetComponent<StickyGrabInteractable>();
        //grabInteractable2 = GetComponent<XRGrabInteractable>();

        //grabInteractable.selectEntered.AddListener(OnGrab);
        //grabInteractable.selectExited.AddListener(OnRelease);

        //grabInteractable2.selectEntered.AddListener(OnGrab);
        //grabInteractable2.selectExited.AddListener(OnRelease);

        //rb.useGravity = false;
        //rb.isKinematic = true;
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if (testGrab)
        {
            onGrab?.Invoke();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!isGrabed)
        {
            onGrab?.Invoke();

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
        //grabInteractable.selectEntered.RemoveListener(OnGrab);
        //grabInteractable.selectExited.RemoveListener(OnRelease);

        //grabInteractable2.selectEntered.RemoveListener(OnGrab);
        //grabInteractable2.selectExited.RemoveListener(OnRelease);
    }
}
