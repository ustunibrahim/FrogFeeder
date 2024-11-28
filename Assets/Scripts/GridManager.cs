using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform map;
    public float xSpacing = 1.0f;
    public float ySpacing = 0.87f;
    public Camera Camera;

    private List<Color> cellColors = new List<Color>
    {
        Color.green, Color.red, Color.yellow, new Color(139, 0, 139), Color.blue,
    };

    public void CreateCellGrid(int rows, int columns)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                float xPos = col * xSpacing;
                float yPos = row * ySpacing;

                GameObject cell = Instantiate(cellPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, map);
                cell.transform.rotation = Quaternion.Euler(-90, 0, 0);

                Color cellColor = cellColors[col % cellColors.Count];
                Renderer renderer = cell.GetComponent<Renderer>();
                renderer.material.color = cellColor;

                TagHelper.AssignColorAndTag(cell, cellColor);
            }
        }

        if (rows == 3) { 
            Camera.transform.position = new Vector3(1, 1.6f, -10);
        }else if (rows == 4)
        {
            Camera.transform.position = new Vector3(1.5f, 2, -10);
        }
    }
}
