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

    // RNG Vars
    [SerializeField] private float interval = 5.0f;
    private int maxNumber;
    private bool isGenerating;
    private int newNumber;

    private void Start()
    {
        isGenerating = false;
        maxNumber = rows * columns;
        StartGenerating();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Comma))
        //{
        //    StartGenerating();
        //}

        //if (Input.GetKeyDown(KeyCode.Period))
        //{
        //    StopGenerating();
        //}
    }

    //private void UpdateGrid(int newRows, int newColumns)
    //{
    //    //rows = newRows;
    //    //columns = newColumns;
    //    //GenerateGrid();
    //}

    #region Inspector Buttons (Generate & Delete Tiles GO)
    public void GenerateGrid()
    {
        DeleteGrid();

        // Calculate the grid Size
        float startX = -(columns - 1) * spacing / 2f;
        float startZ = (rows - 1) * spacing / 2f;

        // Instantiate Tiles
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(startX + column * spacing, 0, startZ - row * spacing) + transform.position;
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Slot ({row},{column})";
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
        // Verifica si todos los GameObjects están activos
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
}
