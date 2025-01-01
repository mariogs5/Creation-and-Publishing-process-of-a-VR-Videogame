using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField, Range(1, 3)] private int maxRows;
    [SerializeField, Range(1, 5)] private int maxColumns;

    private int rows;
    private int columns;

    [SerializeField] private GameObject tilePrefab; // Tile Prefab
    [SerializeField] private float spacing = 1.5f;  // Spacing between tiles

    private List<GameObject> slotPool;

    private void Start()
    {
        //GenerateGrid();
    }

    private void UpdateGrid(int newRows, int newColumns)
    {
        //rows = newRows;
        //columns = newColumns;
        //GenerateGrid();
    }

    #region Inspector Buttons (Generate & Delete Tiles GO)
    public void GenerateGrid()
    {
        DeleteGrid();

        // Calculate the grid Size
        float startX = -(maxColumns - 1) * spacing / 2f;
        float startZ = (maxRows - 1) * spacing / 2f;

        // Instantiate Tiles
        for (int row = 0; row < maxRows; row++)
        {
            for (int column = 0; column < maxColumns; column++)
            {
                Vector3 position = new Vector3(startX + column * spacing, 0, startZ - row * spacing) + transform.position;
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Slot ({row},{column})";
            }
        }
    }

    public void DeleteGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    #endregion
}
