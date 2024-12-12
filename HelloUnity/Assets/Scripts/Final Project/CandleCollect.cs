using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CandleCollect : MonoBehaviour
{
    public TextMeshProUGUI candleNumber;
    private int count = 0;
    private GameObject candle;
    private GameObject fire;
    public GameObject UIending;
    public GameObject ending;
    public TextMeshProUGUI tip;
    public TextMeshProUGUI output;
    public GameObject leftdoor;
    public GameObject rightdoor;
    public GameObject ghost;
    private bool isFirst;
    private bool isOn;
    private bool isLantern = false;
    private GameObject[] lanterns;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        isFirst = true;
        isOn = false;
        candle = GameObject.Find("Character/Hand/Candle");
        fire = GameObject.Find("Character/Hand/Candle/Fire");
        lanterns = GameObject.FindGameObjectsWithTag("Lantern");
        image = UIending.GetComponent<Image>();
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
            bool allLight = true;
            isLantern = false;
            foreach (GameObject lantern in lanterns)
            {
                if (lantern != null) {
                    if (!LightLantern(lantern)){ allLight = false; }
                }
            }
            if (!isLantern)
            {
                tip.text = "";
            }
            if (allLight)
            {
                StartCoroutine(OpenDoor());
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        float elapsedTime = 0f; // Time elapsed since the start
        while (elapsedTime < 3.0f)
        {
            output.text = "The Door is Open!!!\nRun Fast!!!";
            // Calculate the rotation at the current point in time
            float t = elapsedTime / 4.0f;
            rightdoor.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), t);
            leftdoor.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, -90, 0), t);

            elapsedTime += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }
        float distance = Vector3.Distance(transform.position, ending.transform.position);
        float alpha = 0.0f;
        while (distance > 5.0f)
        {
            if (distance < 15.0f)
            {
                ghost.SetActive(false);
            }
            output.text = "";
            distance = Vector3.Distance(transform.position, ending.transform.position);
            alpha = 1.1f - (distance / 25.0f);
            alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        GetComponent<FirstPersonCamera>().enabled = false;
        while (true)
        {
            output.text = "Congradulation!\nYou are safe!!!";
            tip.text = "Press ESC to quit";
            yield return null;
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

    private bool LightLantern(GameObject lantern)
    {
        float distance = Vector3.Distance(transform.position, lantern.transform.position);
        GameObject light = lantern.transform.Find("Point Light").gameObject;
        if (distance < 8.0f)
        {
            isLantern = true;
            tip.text = "Press 'E' to light the lantern";
            if (Input.GetKeyDown(KeyCode.E))
            {
                light.SetActive(true);
                float newIntensity = RenderSettings.ambientIntensity + 0.1f;
                RenderSettings.ambientIntensity = newIntensity;
            }
        }
        return light.activeSelf;
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
