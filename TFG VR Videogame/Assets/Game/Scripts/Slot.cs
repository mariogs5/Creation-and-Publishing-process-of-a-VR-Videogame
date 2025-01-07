using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private float probability = 0.5f;

    private bool noChilds = true;

    private GameObject child1;
    private GameObject child2;




    Transform[] children;

    private void Start()
    {
        children = GetComponentsInChildren<Transform>(true);

        if (children.Length > 2)
        {
            noChilds = false;

            child1 = children[1].gameObject;
            child2 = children[2].gameObject;
        }
    }

    private void OnEnable()
    {
        if (!noChilds)
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
}
