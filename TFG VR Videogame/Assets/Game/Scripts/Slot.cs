using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Slot : MonoBehaviour
{
    // --- Prefab --- \\
    private GameObject molePrefab;
    private GameObject vegetablePrefab;

    // --- RNG Vars --- \\
    private float probability = 0.5f;

    private GameObject child1;
    private GameObject child2;

    private void Awake()
    {
        // New
        //molePrefab = Resources.Load<GameObject>("Prefabs/New Mole Test");
        //vegetablePrefab = Resources.Load<GameObject>("Prefabs/New Vegetable");

        // Old
        child1 = transform.GetChild(0).gameObject;
        child2 = transform.GetChild(1).gameObject;

        child1.SetActive(false);
        child2.SetActive(false);
    }

    private void OnEnable()
    {
        OldEnable();
        //NewEnable();
    }

    private void OldEnable()
    {
        if (child1 != null && child2 != null)
        {
            child1.SetActive(false);
            child2.SetActive(false);

            if (Random.value < probability)
            {
                child1.SetActive(true);
                child2.SetActive(false);
            }
            else
            {
                child1.SetActive(false);
                child2.SetActive(true);
            }
        }
        else
        {
            Debug.Log("There are no Childs in Slot Prefab");
        }
    }

    private void NewEnable()
    {
        if (molePrefab != null && vegetablePrefab != null)
        {
            if (Random.value < probability)
            {
                Debug.Log("Instantiate Mole " + molePrefab);
                Instantiate(molePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Instantiate Vegetable " + vegetablePrefab);
                Instantiate(vegetablePrefab, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("There are no Childs in Slot Prefab");
        }
    }
}
