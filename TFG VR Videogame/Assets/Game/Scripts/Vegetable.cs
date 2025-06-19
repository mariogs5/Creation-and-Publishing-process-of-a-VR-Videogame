using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Vegetable : MonoBehaviour
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
    private bool firstIteration = true;
    private Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.position;

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

        ActivateColorChange();
    }

    private void OnEnable()
    {
        transform.position = initialPos;

        if (!firstIteration)
        {
            ActivateColorChange();
        }
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
                firstIteration = false;
                isActive = false;
                //gameObject.SetActive(false);
                transform.parent.gameObject.SetActive(false); // Deactivate the slot
                //Destroy(gameObject); // New Behavior
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
            gameObject.SetActive(false);
            OnDunk?.Invoke();
            //Destroy(gameObject); // New Behavior
        }
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        //grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}
