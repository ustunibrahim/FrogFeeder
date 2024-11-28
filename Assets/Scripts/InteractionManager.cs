using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;


public class InteractionManager : MonoBehaviour
{
    public TMP_Text moveText;
    public AudioSource frogSound;
    public ParticleSystem frogPS;
    public GameObject tonguePrefab;
    public Transform map; 
    public int move;
    

    

    public void HandleFrogClick(GameObject frog)
    {
        if (move > 0)
        {
            
            frogSound.Play();
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(frog.transform.DOScale(1.5f, 0.2f))
                         .Append(frog.transform.DOScale(1.0f, 0.2f));

            scaleSequence.OnComplete(() => StartCoroutine(ActivateTongue(frog)));

            move--;
            moveText.text = move + " MOVES";

            if (move <= 3 && move !=0)
            {
                moveText.color = Color.red; 
                moveText.text = move + " MOVES";
            }
            else if (move == 0)
            {
                moveText.text = "";
            }
        }
    }

    private IEnumerator ActivateTongue(GameObject frog)
    {
        GameObject tongue = Instantiate(tonguePrefab, frog.transform.position, Quaternion.identity);
        Transform frogTransform = frog.transform;
        string frogTag = frog.tag;
        Vector3 tongueStartPosition = tongue.transform.position;

        bool shouldExpand = true;
        float tongueLength = 0;

        while (shouldExpand)
        {
            GameObject nextGrape = null;
            float closestDistance = float.MaxValue;

            foreach (Transform child in map)
            {
                if (child.CompareTag(frogTag) && child.gameObject.name == "Grape(Clone)")
                {
                    float distance = Vector3.Distance(tongueStartPosition, child.position);

                    if (distance > tongueLength && distance < closestDistance)
                    {
                        Vector3 direction = (child.position - tongueStartPosition).normalized;
                        if (Mathf.Abs(direction.x) < 0.1f)
                        {
                            closestDistance = distance;
                            nextGrape = child.gameObject;
                        }
                    }
                }
            }

            if (nextGrape != null)
            {
                tongueLength = closestDistance;

                tongue.transform.DOScaleY(tongueLength, 0.5f);
                tongue.transform.position = new Vector3(
                    tongueStartPosition.x,
                    tongueStartPosition.y + (tongueLength / 2),
                    tongueStartPosition.z
                );

                yield return new WaitForSeconds(0.2f);

                nextGrape.transform.DOMove(frogTransform.position, 0.5f);
                yield return new WaitForSeconds(0.2f);
                Destroy(nextGrape);
            }
            else
            {
                shouldExpand = false;
            }
        }

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
        yield return new WaitForSeconds(0.5f);

        
        CheckGameState();
    }

    private void CheckGameState()
    {
  
        int remainingFrogs = 0;

        foreach (Transform child in map)
        {
               
            if (child.gameObject.name=="Frog(Clone)") 
            {
                
                remainingFrogs++;
                
            }
        }

        if (move > 0)
        {
            if (remainingFrogs == 0)
            {
                moveText.text = "LEVEL COMPLETED";
                
               
            }
        }
        else if (move == 0)
        {
            if (remainingFrogs == 0) 
            {
                moveText.text = "LEVEL COMPLETED";
               
            }

        }
        else
        {
           
            moveText.text = "GAME OVER";
            moveText.color = Color.red;
        }
    }

  
}
