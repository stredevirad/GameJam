using UnityEngine;
using System.Collections.Generic;

public class BeatDetector : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource musicSource;
    public float bpm = 120f;
    public float firstBeatOffset = 0f;

    [Header("Beat Detection")]
    public float beatThreshold = 0.1f; // Acceptable timing window for hitting a beat

    private float secPerBeat;
    private float songStartTime;
    private float currentBeat = 0;

    public delegate void BeatAction(int beatNumber);
    public static event BeatAction OnBeat;

    void Start()
    {
        secPerBeat = 60f / bpm;
        songStartTime = (float)AudioSettings.dspTime + firstBeatOffset;
        musicSource.Play();
    }

    void Update()
    {
        // Calculate the current beat based on song time
        float currentTime = (float)AudioSettings.dspTime - songStartTime;
        float prevBeat = currentBeat;
        currentBeat = currentTime / secPerBeat;

        // If we've moved to a new beat
        if (Mathf.FloorToInt(prevBeat) != Mathf.FloorToInt(currentBeat))
        {
            if (OnBeat != null)
                OnBeat(Mathf.FloorToInt(currentBeat));
        }
    }

    // Returns how close we are to the nearest beat (-0.5 to 0.5)
    // 0 means perfectly on beat, -0.5 or 0.5 means halfway between beats
    public float GetBeatOffset()
    {
        float currentTime = (float)AudioSettings.dspTime - songStartTime;
        currentBeat = currentTime / secPerBeat;
        return (currentBeat - Mathf.Floor(currentBeat)) - 0.5f;
    }

    // Returns true if player is within the acceptable window for hitting a beat
    public bool IsOnBeat()
    {
        return Mathf.Abs(GetBeatOffset()) < beatThreshold;
    }
}