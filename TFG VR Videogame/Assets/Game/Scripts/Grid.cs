using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField, Range (1,5)] private int sizeZ;
    [SerializeField, Range(1, 3)] private int sizeX;
    private Vector3 size;

    void Start()
    {
        SetGridDimension();
    }

    void Update()
    {
        
    }

    private void SetGridDimension()
    {

        //size.x = sizeX;
        //size.z = sizeZ;
        this.gameObject.transform.localScale = size;
    }

}
