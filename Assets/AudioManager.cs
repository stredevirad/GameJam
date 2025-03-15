using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] levelTracks;
    private AudioSource audioSource;
    private float detectedBPM = 120f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLevelMusic(int level)
    {
        if (level - 1 < levelTracks.Length)
        {
            audioSource.clip = levelTracks[level - 1];
            audioSource.Play();
            StartCoroutine(DetectBPM());
        }
        else
        {
            Debug.LogWarning("No track assigned for this level.");
        }
    }

    IEnumerator DetectBPM()
    {
        yield return new WaitForSeconds(1f);
        detectedBPM = Random.Range(100f, 160f); // Placeholder for actual BPM detection logic
    }

    public float GetTrackBPM()
    {
        return detectedBPM;
    }
}
