using System;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public GridManager gridManager;
    public ElementManager elementManager;
    public InteractionManager interactionManager;
    public int x,y;
    public MainMenu menu;

    internal void RestartLevel(int x, int y)
    {
        gridManager.CreateCellGrid(x, y);
        elementManager.CreateElements(x,y);
    }

    void Start()
    {
        menu.RestartMainMenu();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider != null)
            {
                interactionManager.HandleFrogClick(hit.collider.gameObject);
            }
        }
    }
}
