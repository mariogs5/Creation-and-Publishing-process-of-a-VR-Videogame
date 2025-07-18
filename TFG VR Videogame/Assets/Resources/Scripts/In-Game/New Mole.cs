using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMole : MonoBehaviour
{
    // --- Events --- \\
    public static Action OnHit { get; set; }

    // --- Components --- \\
    private Renderer objectRenderer;

    // --- Color --- \\
    private Color startColor = Color.white;
    private Color endColor = Color.red;
    private float duration = 2f;

    // --- GO Vars --- \\
    private float timer;
    private bool isActive = false;

    private void Awake()
    {
        //objectRenderer = GetComponent<Renderer>();
        //if (objectRenderer != null)
        //{
        //    objectRenderer.material.color = startColor;
        //}

        isActive = true;    

        ActivateColorChange();
    }

    void Update()
    {
        //Timer to lose points

        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer <= duration)
            {
                //float t = timer / duration;
                //objectRenderer.material.color = Color.Lerp(startColor, endColor, t);
            }
            else
            {
                isActive = false;
                Destroy(gameObject);
            }
        }
    }

    public void ActivateColorChange()
    {
        //objectRenderer.material.color = startColor;
        isActive = true;
        timer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mace")
        {
            OnHit?.Invoke();
            Destroy(gameObject);
        }
    }
}
