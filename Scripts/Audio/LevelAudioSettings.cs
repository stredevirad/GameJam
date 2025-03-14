using UnityEngine;

public class LevelAudioSettings : MonoBehaviour
{
    [Header("Music Settings")]
    public float bpm = 120f;
    public float firstBeatOffset = 0f;
    public float beatThreshold = 0.1f;

    [Header("Beat Markers")]
    public float[] beatMarkers; // Array of beat positions in seconds

    [Header("Section Markers")]
    public float[] sectionMarkers; // Array of section boundaries in seconds

    void Start()
    {
        // Set up beat detector
        BeatDetector beatDetector = FindObjectOfType<BeatDetector>();
        if (beatDetector != null)
        {
            beatDetector.bpm = bpm;
            beatDetector.firstBeatOffset = firstBeatOffset;
            beatDetector.beatThreshold = beatThreshold;
        }
    }
}