using UnityEngine;

public class MainController : MonoBehaviour
{
    public GridManager gridManager;
    public ElementManager elementManager;
    public InteractionManager interactionManager;

    void Start()
    {
        gridManager.CreateCellGrid(5, 5);
        elementManager.CreateElements(5, 5);
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
