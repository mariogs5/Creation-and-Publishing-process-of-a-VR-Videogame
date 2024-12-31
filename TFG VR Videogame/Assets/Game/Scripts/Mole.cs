using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private Mallet mallet;

    void Start()
    {
        mallet.onHit += Kill;
    }

    void Update()
    {
        
    }

    private void Kill()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        mallet.onHit += Kill;
    }
    private void OnDisable()
    {
        mallet.onHit -= Kill;
    }
}
