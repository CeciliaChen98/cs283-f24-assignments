using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject character;
    public GameObject ghost;
    public GameObject uiPanel;
    private Image die;
    //private Vector4 color;

    void Start()
    {
        die = uiPanel.GetComponent<Image>();
        die.color = new Color(die.color.r, die.color.g, die.color.b, 0.0f);
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.volume = 0.5f; // Set volume to 50%
            audioSource.pitch = 1.0f;
        }
        else
        {
            Debug.LogWarning("No AudioSource found on this GameObject.");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(character.transform.position, ghost.transform.position);

        float volume = 1.2f - (distance / 80.0f);
        float alpha = 0.55f - (distance / 60.0f);
        float pitch = 1.7f - (distance / 100.0f);

        volume = Mathf.Clamp(volume, 0.3f, 1.0f);
        pitch = Mathf.Clamp(pitch, 1.0f, 1.5f);
        alpha = Mathf.Clamp(alpha, 0.0f, 0.5f);

        audioSource.volume = volume;
        audioSource.pitch = pitch;
        die.color = new Color(die.color.r, die.color.g, die.color.b, alpha);

    }
}