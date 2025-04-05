using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    public static Action OnDunk { get; set; }

    private Color startColor = Color.white;
    private Color endColor = Color.green;
    private float duration = 2f;

    private Renderer objectRenderer;
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
                transform.parent.gameObject.SetActive(false);
            }
        }
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
        }
    }
}
