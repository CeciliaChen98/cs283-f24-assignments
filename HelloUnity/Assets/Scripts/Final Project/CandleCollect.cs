using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandleCollect : MonoBehaviour
{
    public TextMeshProUGUI candleNumber;
    private int count = 0;
    private GameObject candle;
    private GameObject fire;
    public TextMeshProUGUI tip;
    private bool isFirst;
    private bool isOn;
    private bool isLantern = false;
    private GameObject[] lanterns;

    // Start is called before the first frame update
    void Start()
    {
        isFirst = true;
        isOn = false;
        candle = GameObject.Find("Character/Hand/Candle");
        fire = GameObject.Find("Character/Hand/Candle/Fire");
        lanterns = GameObject.FindGameObjectsWithTag("Lantern");
    }

    void Update()
    {
        if(count == 0)
        {
            candle.SetActive(false);
        }
        else
        {
            candle.SetActive(true);
            if (isFirst)
            {
                fire.SetActive(false);
                isOn = false;
                tip.text = "Press 'F' to use the candle";
                isFirst = false;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isOn) { isOn = true;  fire.SetActive(true); }
                else { isOn = false; fire.SetActive(false); }
                tip.text = "";
            }
        }
        if(isOn)
        {
            isLantern = false;
            foreach (GameObject lantern in lanterns)
            {
                if (lantern != null) { LightLantern(lantern); }
            }
            if (!isLantern)
            {
                tip.text = "";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candle"))
        {
            count++;
            candleNumber.text = count.ToString();
            StartCoroutine(TriggerCollectEffect(other.gameObject));
        }
    }

    private void LightLantern(GameObject lantern)
    {
        float distance = Vector3.Distance(transform.position, lantern.transform.position);
        if (distance < 8.0f)
        {
            isLantern = true;
            tip.text = "Press 'E' to light the lantern";
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject light = lantern.transform.Find("Point Light").gameObject;
                light.SetActive(true);
            }
        }
    }

    private IEnumerator TriggerCollectEffect(GameObject candle)
    {
        float duration = 0.6f;
        float elapsed = 0f;

        Vector3 originalScale = candle.transform.localScale;
        Vector3 originalPosition = candle.transform.position;
        float distance = 5.0f;

        while (elapsed < duration)
        {
            // Rotate the object
            candle.transform.Rotate(Vector3.up, 60 * Time.deltaTime);

            float verticalMovement = Mathf.Lerp(0, distance, elapsed / duration);
            candle.transform.localPosition = originalPosition + (transform.up * verticalMovement) + (transform.forward * verticalMovement);

            // Increase elapsed time
            elapsed += Time.deltaTime;

            yield return null; // Wait for the next frame
        }
        candle.SetActive(false);
    }
}
