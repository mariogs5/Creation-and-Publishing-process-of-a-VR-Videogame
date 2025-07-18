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

    Vector3 posMole;
    Vector3 posVeg;

    private void Awake()
    {
        // --- Test Prefabs --- \\
        //molePrefab = Resources.Load<GameObject>("Prefabs/Test/New Mole Test");
        //vegetablePrefab = Resources.Load<GameObject>("Prefabs/Test/New Vegetable");

        // --- Final Models --- \\
        molePrefab = Resources.Load<GameObject>("Prefabs/Final/Mole");
        vegetablePrefab = Resources.Load<GameObject>("Prefabs/Final/Naboncio");

        posMole = new Vector3(transform.position.x, (transform.position.y - (float)0.3), transform.position.z);
        posVeg = new Vector3(transform.position.x, (transform.position.y - (float)0.35), transform.position.z);
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
                Instantiate(molePrefab, posMole, transform.rotation);
            }
            else
            {
                Debug.Log("Instantiate Vegetable ");
                Instantiate(vegetablePrefab, posVeg, transform.rotation);
            }
        }
        else
        {
            Debug.Log("There are no Childs in Slot Prefab");
        }
    }
}
