using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private Mallet mallet;

    void Start()
    {
        if(mallet != null)
        {
            mallet.OnHit += Kill;
        }
    }

    void Update()
    {
        
    }

    private void Kill()
    {
        gameObject.SetActive(false);
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (mallet != null)
        {
            mallet.OnHit += Kill;
        }
    }
    private void OnDisable()
    {
        if (mallet != null)
        {
            mallet.OnHit -= Kill;
        }
    }
}
