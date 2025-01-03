using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public static Action OnHit {  get; set; }

    private void Awake()
    {
        //Do animation
    }

    void Update()
    {
        //Timer to lose points
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
