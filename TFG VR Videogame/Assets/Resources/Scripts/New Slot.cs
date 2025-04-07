using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class NewSlot : MonoBehaviour
{
    // --- Prefab --- \\
    private GameObject molePrefab;
    private GameObject vegetablePrefab;

    // --- RNG Vars --- \\
    private float probability = 0.5f;

    private void Awake()
    {
        molePrefab = Resources.Load<GameObject>("Prefabs/New Mole Test");
        vegetablePrefab = Resources.Load<GameObject>("Prefabs/New Vegetable");
    }

    private void OnEnable()
    {
        //SpawnObject();
    }

    public void SpawnObject()
    {
        if (molePrefab != null && vegetablePrefab != null)
        {
            if (Random.value < probability)
            {
                Debug.Log("Instantiate Mole ");
                Instantiate(molePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Instantiate Vegetable ");
                Instantiate(vegetablePrefab, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("There are no Childs in Slot Prefab");
        }
    }
}
