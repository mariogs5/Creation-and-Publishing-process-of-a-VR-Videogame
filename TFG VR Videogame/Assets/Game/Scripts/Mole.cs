using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public static Action OnHit {  get; set; }

    private Color startColor = Color.white;
    private Color endColor = Color.red;    
    private float duration = 2f;        

    private Renderer objectRenderer;
    private float timer;
    private bool isActive = false;
    private bool firstIteration = true;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = startColor;
        }

        ActivateColorChange();
    }

    private void OnEnable()
    {
        //Do animation
        if (!firstIteration)
        {
            ActivateColorChange();
        }
    }

    void Update()
    {
        //Timer to lose points


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
                gameObject.SetActive(false);
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
        if(collision.gameObject.tag == "Mallet")
        {
            gameObject.SetActive(false);
            OnHit?.Invoke();
        }
    }
}
