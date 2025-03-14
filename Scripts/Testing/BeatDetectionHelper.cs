using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// This tool helps you determine the BPM and beat offset of music tracks
public class BeatDetectionHelper : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicClip;
    public Button tapButton;
    public Text bpmText;
    public Text offsetText;

    private List<float> tapTimes = new List<float>();
    private float firstTapTime = 0;
    private float startTime = 0;

    void Start()
    {
        if (tapButton != null)
        {
            tapButton.onClick.AddListener(OnTapButton);
        }

        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
        }
    }

    public void PlayMusic()
    {
        if (musicSource != null)
        {
            musicSource.Play();
            startTime = Time.time;
            tapTimes.Clear();
            UpdateDisplay();
        }
    }

    void OnTapButton()
    {
        float tapTime = Time.time - startTime;
        tapTimes.Add(tapTime);

        if (tapTimes.Count == 1)
        {
            firstTapTime = tapTime;
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // Need at least 4 taps to calculate BPM
        if (tapTimes.Count < 4)
        {
            if (bpmText != null)
                bpmText.text = "BPM: Need more taps";
            if (offsetText != null)
                offsetText.text = "Offset: " + firstTapTime.ToString("F3") + "s";
            return;
        }

        // Calculate average interval between taps
        float totalIntervals = 0;
        int numIntervals = 0;

        for (int i = 1; i < tapTimes.Count; i++)
        {
            totalIntervals += tapTimes[i] - tapTimes[i - 1];
            numIntervals++;
        }

        float averageInterval = totalIntervals / numIntervals;
        float calculatedBPM = 60f / averageInterval;

        if (bpmText != null)
            bpmText.text = "BPM: " + calculatedBPM.ToString("F2");

        if (offsetText != null)
            offsetText.text = "Offset: " + firstTapTime.ToString("F3") + "s";
    }
}