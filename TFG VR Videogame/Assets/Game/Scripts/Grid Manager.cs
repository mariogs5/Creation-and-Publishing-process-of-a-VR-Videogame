using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField, Range(1, 3)] private int rows;
    [SerializeField, Range(1, 5)] private int columns;

    [SerializeField] private GameObject tilePrefab; // Tile Prefab
    [SerializeField] private float spacing = 1.5f;  // Spacing between tiles

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        DeleteGrid();

        // Calculate the grid Size
        float startX = -(columns - 1) * spacing / 2f;
        float startY = -(rows - 1) * spacing / 2f;

        // Instantiate Tiles
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(startX + col * spacing, startY + row * spacing, 0) + transform.position;
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Slot ({row},{col})";

                // Color Test
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                Material randomMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                randomMaterial.color = randomColor;
                tile.GetComponent<Renderer>().material = randomMaterial;
            }
        }
    }

    // Método para actualizar la grid dinámicamente
    private void UpdateGrid(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;
        GenerateGrid();
    }

    public void DeleteGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
