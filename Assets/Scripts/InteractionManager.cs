using UnityEngine;
using DG.Tweening;
using System.Collections;

public class InteractionManager : MonoBehaviour
{
    public AudioSource frogSound;
    public ParticleSystem frogPS;
    public GameObject tonguePrefab;
    public Transform map;

    public void HandleFrogClick(GameObject frog)
    {
        frogSound.Play();
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(frog.transform.DOScale(1.5f, 0.2f))
                     .Append(frog.transform.DOScale(1.0f, 0.2f));

        scaleSequence.OnComplete(() => StartCoroutine(ActivateTongue(frog)));

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
        /*GameObject tongue = Instantiate(tonguePrefab, frog.transform.position, Quaternion.identity);
        // Tongue movement logic goes here

        yield return new WaitForSeconds(0.5f);
        Destroy(tongue);
        frogPS.transform.position = frog.transform.position;
        Destroy(frog);
        frogPS.Play();*/
    }
}
