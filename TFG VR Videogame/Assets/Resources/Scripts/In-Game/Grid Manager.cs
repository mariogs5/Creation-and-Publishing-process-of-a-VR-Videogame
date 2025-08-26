using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // --- Grid Vars --- \\
    [SerializeField, Range(1, 3)] private int rows;
    [SerializeField, Range(1, 5)] private int columns;
    [SerializeField] private float spacing = 1.5f;  // Spacing between tiles

    // --- Model Vars --- \\
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<GameObject> slotPool;
    private Vector3 tileSize;

    // --- RNG Vars --- \\
    [SerializeField] private float interval;
    private int maxNumber;
    private bool isGenerating;
    private int newNumber;

    private Queue<int> numberQueue = new Queue<int>(3);
    private HashSet<int> numberSet = new HashSet<int>();

    private void Start()
    {
        isGenerating = false;
        maxNumber = rows * columns;
        //StartGeneratingNumbers();
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

                //tile.SetActive(false);
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
    private void StartGeneratingNumbers()
    {
        if (!isGenerating)
        {
            InvokeRepeating(nameof(GenerateRandomNumber), 0, interval);
            isGenerating = true;
            Debug.Log("Starting RNG Slot Number Generation.");
        }
    }

    private void StopGeneratingNumbers()
    {
        if (isGenerating)
        {
            CancelInvoke(nameof(GenerateRandomNumber));
            isGenerating = false;
            Debug.Log("Stopping RNG Slot Number Generation.");
        }
    }

    private void GenerateRandomNumber()
    {
        // Old 
        //if (slotPool.TrueForAll(go => go.activeSelf))
        //{
        //    StopGeneratingNumbers();
        //    return;
        //}

        //do
        //{
        //    newNumber = Random.Range(0, maxNumber);
        //} while (slotPool[newNumber].activeSelf);

        //Debug.Log("Number Generated: " + newNumber);

        //slotPool[newNumber].SetActive(true);

        // New 
        do
        {
            newNumber = Random.Range(0, maxNumber);
        } while (numberSet.Contains(newNumber));

        // Update data structures
        numberQueue.Enqueue(newNumber);
        numberSet.Add(newNumber);

        if (numberQueue.Count > 3)
        {
            int oldestNumber = numberQueue.Dequeue();
            numberSet.Remove(oldestNumber);
        }

        Debug.Log("Number Generated: " + newNumber);

        //slotPool[newNumber].GetComponent<NewSlot>().SpawnObject();
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
