using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellGrid : MonoBehaviour
{
    public GameObject cellPrefab, frogPrefab, grapePrefab, TonguePrefab;
    public float xSpacing = 1.0f;
    public float ySpacing = 0.87f;
    public Transform map;
    public ParticleSystem frogPS;
    public AudioSource frogSound;


    private List<Color> cellColors = new List<Color>
    {
        Color.green,
        Color.red,
        Color.yellow,
        new Color(139,0,139), // purple
        Color.blue,
    };

    void Start()
    {
        CreateCellGrid(5, 5);
    }

    void CreateCellGrid(int rows, int columns)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                float xPos = col * xSpacing;
                float yPos = row * ySpacing;

               

               
                GameObject cell = Instantiate(cellPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, map);
                Renderer renderer = cell.GetComponent<Renderer>();
                cell.transform.rotation = Quaternion.Euler(-90, 0, 0);

                if (col == 0)
                {
                    renderer.material.color = Color.green;
                    AssignColorAndTag(cell, Color.green);
                }
                else if (col == 1)
                {
                    renderer.material.color = Color.red;
                    AssignColorAndTag(cell, Color.red);
                }
                else if (col == 2)
                {
                    renderer.material.color = new Color(139, 0, 139);
                    AssignColorAndTag(cell, new Color(139, 0, 139));
                }
                else if (col == 3)
                {
                    renderer.material.color = Color.yellow;
                    AssignColorAndTag(cell, Color.yellow);
                }
                else if (col == 4)
                {
                    renderer.material.color = Color.blue;
                    AssignColorAndTag(cell, Color.blue);
                }

            }
        }

        CreateElements(rows, columns);
    }

    void CreateElements(int rows, int columns)
    {
        if (map != null)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    float xPos = col * xSpacing;
                    float yPos = row * ySpacing;

                    

                    if (row == 0)
                    {
                        GameObject frog = Instantiate(frogPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity, map);
                        frog.transform.rotation = Quaternion.Euler(90, 180, 0);

                        foreach (Transform cell in map)
                        {
                            if (Vector3.Distance(cell.position, new Vector3(xPos, yPos, 0)) < 0.1f)
                            {
                                frog.tag = cell.tag;
                                break;
                            }
                        }
                        AssignFrogMaterialBasedOnTag(frog);
                    }
                    else
                    {
                        GameObject grape = Instantiate(grapePrefab, new Vector3(xPos, yPos, -.2f), Quaternion.identity, map);
                        foreach (Transform cell in map)
                        {
                            if (Vector3.Distance(cell.position, new Vector3(xPos, yPos, 0)) < 0.1f)
                            {
                                grape.tag = cell.tag;
                                break;
                            }
                        }
                        AssignGrapeMaterialBasedOnTag(grape);
                    }

                }
            }
        }
    }

    void AssignColorAndTag(GameObject cellPrefab, Color color)
    {
        Renderer hexRenderer = cellPrefab.GetComponent<Renderer>();
        hexRenderer.material.color = color;

        if (color == Color.red)
        {
            cellPrefab.tag = "Red";
        }
        else if (color == Color.blue)
        {
            cellPrefab.tag = "Blue";
        }
        else if (color == Color.green)
        {
            cellPrefab.tag = "Green";
        }
        else if (color == Color.yellow)
        {
            cellPrefab.tag = "Yellow";
        }
        else if (color == new Color(139, 0, 139))
        {
            cellPrefab.tag = "Purple";
        }
        else
        {
            cellPrefab.tag = "Untagged"; 
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol fare tıklaması
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Fare konumundan ray oluştur
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    OnMouseDown(hit.collider.gameObject);
                }
            }
        }
    }

    // Frog üzerine tıklama işlemi
    void OnMouseDown(GameObject frog)
    {
        frogSound.Play();

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(frog.transform.DOScale(1.5f, 0.2f)) // Biraz büyüt
        .Append(frog.transform.DOScale(1.0f, 0.2f)); // Eski haline getir

        // Büyütme animasyonu tamamlandıktan sonra dil hareketini başlat
        scaleSequence.OnComplete(() =>
        {
            StartCoroutine(ActivateTongue(frog));
        });
    }

    IEnumerator ActivateTongue(GameObject frog)
    {
        GameObject tongue = Instantiate(TonguePrefab, frog.transform.position, Quaternion.identity);
        Transform frogTransform = frog.transform;
        string frogTag = frog.tag;
        Vector3 tongueStartPosition = tongue.transform.position;

        bool shouldExpand = true;
        float tongueLength = 0;

        while (shouldExpand)
        {
            GameObject nextGrape = null;
            float closestDistance = float.MaxValue;

            // Grapes arasında doğrusal olarak en yakınını bul
            foreach (Transform child in map)
            {
                if (child.CompareTag(frogTag) && child.gameObject.name == "Grape(Clone)")
                {
                    float distance = Vector3.Distance(tongueStartPosition, child.position);

                    // Tongue'un mevcut uzunluğunun ötesindeki grape'leri kontrol et
                    if (distance > tongueLength && distance < closestDistance)
                    {
                        // Yön kontrolü (örneğin, yukarı doğru hareket ediyorsa x koordinatını sabit tutarız)
                        Vector3 direction = (child.position - tongueStartPosition).normalized;
                        if (Mathf.Abs(direction.x) < 0.1f) // Dikey doğrultudaki grape'leri seç
                        {
                            closestDistance = distance;
                            nextGrape = child.gameObject;
                        }
                    }
                }
            }

            if (nextGrape != null)
            {
                // Dil uzamaya devam ediyor
                tongueLength = closestDistance;

                // Tongue'u büyüt ve pozisyonunu ayarla
                tongue.transform.DOScaleY(tongueLength, 0.5f);
                tongue.transform.position = new Vector3(
                    tongueStartPosition.x,
                    tongueStartPosition.y + (tongueLength / 2),
                    tongueStartPosition.z
                );

                yield return new WaitForSeconds(0.2f);

                // Grape'e ulaştıysa onu yok et
                nextGrape.transform.DOMove(frogTransform.position, 0.5f);
                yield return new WaitForSeconds(0.2f);
                Destroy(nextGrape);
            }
            else
            {
                // Eğer uygun bir grape yoksa veya tüm grape'ler tüketildiyse, geri çekil
                shouldExpand = false;
            }
        }

        // Dil küçültme animasyonu
        tongue.transform.DOScaleY(0, 0.5f).OnUpdate(() =>
        {
            tongue.transform.position = new Vector3(
                tongueStartPosition.x,
                tongueStartPosition.y + (tongue.transform.localScale.y / 2),
                tongueStartPosition.z
            );
        });

        yield return new WaitForSeconds(0.5f);
        Destroy(tongue);
        frogPS.transform.position = frog.transform.position;
        Destroy(frog);
        frogPS.Play();
    }



}
