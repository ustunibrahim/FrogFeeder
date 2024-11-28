using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public GameObject frogPrefab, grapePrefab;
    public Transform map;

    public float xSpacing = 1.0f;
    public float ySpacing = 0.87f;

    private Dictionary<string, Material> frogMaterials = new Dictionary<string, Material>();
    private Dictionary<string, Material> grapeMaterials = new Dictionary<string, Material>();

    void Start()
    {
        LoadMaterials();
    }

    public void CreateElements(int rows, int columns)
    {
        if (map == null)
        {
            Debug.LogError("Map transform is not assigned!");
            return;
        }

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                float xPos = col * xSpacing;
                float yPos = row * ySpacing;

                if (row == 0) // İlk sıra: Frog'lar
                {
                    GameObject frog = Instantiate(frogPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, map);
                    frog.transform.rotation = Quaternion.Euler(90, 180, 0);

                    AssignTagAndMaterial(frog, xPos, yPos, "Frog");
                }
                else // Diğer sıralar: Grape'ler
                {
                    GameObject grape = Instantiate(grapePrefab, new Vector3(xPos, yPos, -0.2f), Quaternion.identity, map);
                    AssignTagAndMaterial(grape, xPos, yPos, "Grape");
                }
            }
        }
    }

    private void LoadMaterials()
    {
        // Resources/Materials/FrogMaterials ve GrapeMaterials klasörlerinden materyalleri yükle
        Material[] loadedFrogMaterials = Resources.LoadAll<Material>("Materials/FrogMaterials");
        foreach (Material mat in loadedFrogMaterials)
        {
            frogMaterials[mat.name] = mat;
        }

        Material[] loadedGrapeMaterials = Resources.LoadAll<Material>("Materials/GrapeMaterials");
        foreach (Material mat in loadedGrapeMaterials)
        {
            
            grapeMaterials[mat.name] = mat;
        }
    }

    private void AssignTagAndMaterial(GameObject element, float xPos, float yPos, string type)
    {
        string cellTag = FindNearestCellTag(new Vector3(xPos, yPos, 0));

        if (!string.IsNullOrEmpty(cellTag))
        {
            element.tag = cellTag;

            // Materyal ataması
            Material material = null;
            Material[] loadedFrogMaterials = Resources.LoadAll<Material>("Materials/FrogMaterials");
            Material[] loadedGrapeMaterials = Resources.LoadAll<Material>("Materials/GrapeMaterials");


            if (type == "Frog" )
            {
                
                    AssignFrogMaterialBasedOnTag(element);
                
            }
            else if (type == "Grape")
            {
                    AssignGrapeMaterialBasedOnTag(element);
            }
        }
    }

    private string FindNearestCellTag(Vector3 position)
    {
        string nearestTag = "";
        float minDistance = float.MaxValue;

        foreach (Transform cell in map)
        {
            float distance = Vector3.Distance(position, cell.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTag = cell.tag;
            }
        }

        return nearestTag;
    }

    void AssignGrapeMaterialBasedOnTag(GameObject grape)
    {
        string grapeTag = grape.tag + "Grape";

        Material material = Resources.Load<Material>("Materials/GrapeMaterials/" + grapeTag);

        if (material != null)
        {
            Renderer frogRenderer = grape.GetComponent<Renderer>();
            if (frogRenderer != null)
            {
                frogRenderer.material = material;
            }
        }
        else
        {
            Debug.LogWarning("Materyal bulunamadı: " + grapeTag);
        }
    }

    public void AssignFrogMaterialBasedOnTag(GameObject frog)
    {
        string frogTag = frog.tag + "Frog";

        Material material = Resources.Load<Material>("Materials/FrogMaterials/" + frogTag);

        if (material != null)
        {
            Transform cubeTransform = frog.transform.Find("Cube.012");
            if (cubeTransform != null)
            {
                Renderer cubeRenderer = cubeTransform.GetComponent<Renderer>();
                if (cubeRenderer != null)
                {
                    cubeRenderer.material = material;
                }
            }
            else
            {
                Debug.LogWarning($"Cube.012 alt nesnesi {frog.name} içinde bulunamadı.");
            }
        }
        else
        {
            Debug.LogWarning("Materyal bulunamadı: " + frogTag);
        }
    }


}
