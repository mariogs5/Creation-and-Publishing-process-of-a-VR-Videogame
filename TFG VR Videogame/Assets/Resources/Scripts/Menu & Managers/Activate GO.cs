using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateGO : MonoBehaviour
{
    [SerializeField] private GameObject moles;

    private void OnEnable()
    {
        Mace.onGrab += StartMenu;
    }
    private void OnDisable()
    {
        Mace.onGrab -= StartMenu;
    }

    private void StartMenu()
    {
        if (moles != null)
        {
            moles.SetActive(true);
        }
    }
}
