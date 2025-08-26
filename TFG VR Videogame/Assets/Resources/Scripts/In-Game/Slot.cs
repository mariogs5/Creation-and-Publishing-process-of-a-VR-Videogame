using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Slot : MonoBehaviour
{
    // --- Prefab --- \\
    private GameObject molePrefab;
    private GameObject vegetablePrefab;

    Vector3 posMole;
    Vector3 posVeg;

    public bool isMole;
    public float lifeTime;

    private void Awake()
    {
        // --- Final Models --- \\
        molePrefab = Resources.Load<GameObject>("Prefabs/Final/In Game/Mole");
        vegetablePrefab = Resources.Load<GameObject>("Prefabs/Final/In Game/Naboncio");

        //posMole = new Vector3(transform.position.x, (transform.position.y - (float)0.3), transform.position.z);
        //posVeg = new Vector3(transform.position.x, (transform.position.y - (float)0.35), transform.position.z);
    }

    private void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        if(molePrefab != null && vegetablePrefab != null)
        {
            if (isMole)
            {
                GameObject obj = Instantiate(molePrefab);
                obj.transform.SetParent(gameObject.transform, false);
                obj.transform.localPosition = gameObject.transform.position;
                //obj.transform.localPosition = posMole;
                //obj.transform.localRotation = Quaternion.identity;

                Mole moleScript = obj.GetComponent<Mole>();
                moleScript.duration = lifeTime;
            }
            else
            {
                GameObject obj = Instantiate(vegetablePrefab);
                obj.transform.SetParent(gameObject.transform, false);
                obj.transform.localPosition = gameObject.transform.position;
                //obj.transform.localPosition = posVeg;
                //obj.transform.localRotation = Quaternion.identity;

                Vegetable vegScript = obj.GetComponent<Vegetable>();
                //vegScript.duration = lifeTime;
            }
        }
        else
        {
            Debug.Log("One or more of the GOs of the slot is missing");
        }
    }
}
