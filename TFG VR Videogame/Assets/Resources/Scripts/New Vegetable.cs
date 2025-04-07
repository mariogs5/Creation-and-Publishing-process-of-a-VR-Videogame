using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NewVegetable : MonoBehaviour
{
    // --- Events --- \\
    public static Action OnDunk { get; set; }

    // --- Components --- \\
    private Renderer objectRenderer;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    // --- Color --- \\
    private Color startColor = Color.white;
    private Color endColor = Color.green;
    private float duration = 2f;

    // --- GO Vars --- \\
    private float timer;
    private bool isActive = false;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = startColor;
        }

        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        //grabInteractable.selectExited.AddListener(OnRelease);

        rb.useGravity = false;

        isActive = true;

        ActivateColorChange();
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer <= duration)
            {
                float t = timer / duration;
                objectRenderer.material.color = Color.Lerp(startColor, endColor, t);
            }
            else
            {
                isActive = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void ActivateColorChange()
    {
        objectRenderer.material.color = startColor;
        isActive = true;
        timer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "VeggieBasket")
        {
            OnDunk?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        //grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
