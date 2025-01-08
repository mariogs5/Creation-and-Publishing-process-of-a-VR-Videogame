using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Grid Vars
    [SerializeField, Range(1, 3)] private int rows;
    [SerializeField, Range(1, 5)] private int columns;
    [SerializeField] private float spacing = 1.5f;  // Spacing between tiles

    // Model Vars
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<GameObject> slotPool;
    private Vector3 tileSize;

    // RNG Vars
    [SerializeField] private float interval;
    private int maxNumber;
    private bool isGenerating;
    private int newNumber;

    private void Start()
    {
        isGenerating = false;
        maxNumber = rows * columns;
        StartGenerating();
    }

    #region Inspector Buttons (Generate & Delete Tiles GO)
    public void GenerateGrid()
    {
        DeleteGrid();

        CalculateTileSize();

        float startX = -(columns - 1) * (tileSize.x + spacing) / 2f;
        float startZ = (rows - 1) * (tileSize.z + spacing) / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(startX + column * (tileSize.x + spacing), 0, startZ - row * (tileSize.z + spacing)) + transform.position;

                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Slot ({row},{column})";

                // Prueba de codigo
                //Transform[] children = tile.GetComponentsInChildren<Transform>(true);

                //if (children.Length > 2)
                //{

                //    GameObject child1 = children[1].gameObject;
                //    GameObject child2 = children[2].gameObject;

                //    child1.SetActive(false);
                //    child2.SetActive(false);
                //}
                //else
                //{
                //    tile.SetActive(false);
                //}

                tile.SetActive(false);
                slotPool.Add(tile);
            }
        }
    }

    public void DeleteGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        slotPool.Clear();
    }
    #endregion

    #region Number Generation
    private void StartGenerating()
    {
        if (!isGenerating)
        {
            InvokeRepeating(nameof(GenerateRandomNumber), 0, interval);
            isGenerating = true;
            Debug.Log("Starting RNG Slot Number Generation.");
        }
    }

    private void GenerateRandomNumber()
    {
        if (slotPool.TrueForAll(go => go.activeSelf))
        {
            StopGenerating();
            return;
        }

        do
        {
            newNumber = Random.Range(0, maxNumber);
        } while (slotPool[newNumber].activeSelf);

        Debug.Log("Number Generated: " + newNumber);

        //Slot slot = slotPool[newNumber].GetComponent<Slot>();
        //slot.OnActivate();
        slotPool[newNumber].SetActive(true);
    }

    private void StopGenerating()
    {
        if (isGenerating)
        {
            CancelInvoke(nameof(GenerateRandomNumber));
            isGenerating = false;
            Debug.Log("Stopping RNG Slot Number Generation.");
        }
    }
    #endregion

    #region Helper Methods
    private void CalculateTileSize()
    {
        Renderer renderer = tilePrefab.GetComponent<Renderer>();
        if (renderer != null)
        {
            tileSize = renderer.bounds.size;
        }
        else
        {
            Collider collider = tilePrefab.GetComponent<Collider>();
            if (collider != null)
            {
                tileSize = collider.bounds.size;
            }
            else
            {
                Debug.Log("Tile Prefab must have a Renderer or Collider to calculate its size.");
                tileSize = new Vector3(0.2f, 0.5f, 0.2f);
            }
        }
    }
    #endregion
}
