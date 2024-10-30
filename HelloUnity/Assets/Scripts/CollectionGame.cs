using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionGame : MonoBehaviour
{
    public TextMeshProUGUI score;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        score.text = count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            count++; // Increment the count
            score.text = count.ToString();
            StartCoroutine(TriggerCollectEffect(other.gameObject));
        }
    }

    private IEnumerator TriggerCollectEffect(GameObject collectable)
    {
        float duration = 0.6f; 
        float elapsed = 0f; 

        Vector3 originalScale = collectable.transform.localScale;
        Vector3 originalPosition = collectable.transform.position;
        float distance = 5.0f;

        while (elapsed < duration)
        {
            // Rotate the object
            collectable.transform.Rotate(Vector3.up, 60 * Time.deltaTime);

            // Shrink the object
            float scale = Mathf.Lerp(1f,2.0f, elapsed / duration);
            collectable.transform.localScale = originalScale * scale;

            float verticalMovement = Mathf.Lerp(0, distance, elapsed / duration);
            collectable.transform.position = originalPosition + Vector3.up * verticalMovement;

            // Increase elapsed time
            elapsed += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Deactivate the collectable object after the effect
        collectable.SetActive(false);
    }
}
