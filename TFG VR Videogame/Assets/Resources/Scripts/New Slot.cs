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
        // --- Test Prefabs --- \\
        molePrefab = Resources.Load<GameObject>("Prefabs/Test/New Mole Test");
        vegetablePrefab = Resources.Load<GameObject>("Prefabs/Test/New Vegetable");

        // --- Final Models --- \\
        //molePrefab = Resources.Load<GameObject>("Prefabs/Final/Mole");
        //vegetablePrefab = Resources.Load<GameObject>("Prefabs/Final/Naboncio");
    }

    private void OnEnable()
    {
        //SpawnObject();
    }

    public void SpawnObject()
    {
        if (molePrefab != null && vegetablePrefab != null)
        {
            Vector3 pos = new Vector3(transform.position.x, (transform.position.y - (float)0.3), transform.position.z);

            if (Random.value < probability)
            {
                Debug.Log("Instantiate Mole ");
                Instantiate(molePrefab, pos, Quaternion.identity);
            }
            else
            {
                Debug.Log("Instantiate Vegetable ");
                Instantiate(vegetablePrefab, pos, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("There are no Childs in Slot Prefab");
        }
    }
}
