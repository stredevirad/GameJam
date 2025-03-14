using UnityEngine;
using System.Collections.Generic;

public class GameTester : MonoBehaviour
{
    [Header("Test Settings")]
    public bool autoPlay = false;
    public float perfectTimingChance = 0.9f;
    public float missChance = 0.1f;

    [Header("Debug Info")]
    public bool showDebugInfo = true;
    public bool logBeats = true;

    private BeatDetector beatDetector;
    private PlayerController player;
    private List<float> beatTimes = new List<float>();
    private float lastBeatTime = 0f;

    void Start()
    {
        beatDetector = FindObjectOfType<BeatDetector>();
        player = FindObjectOfType<PlayerController>();

        // Subscribe to beat events
        BeatDetector.OnBeat += OnBeat;
    }

    void OnDestroy()
    {
        // Unsubscribe from beat events
        BeatDetector.OnBeat -= OnBeat;
    }

    void OnBeat(int beatNumber)
    {
        if (logBeats)
        {
            Debug.Log("Beat: " + beatNumber);
        }

        lastBeatTime = Time.time;
        beatTimes.Add(lastBeatTime);

        // Auto-play functionality
        if (autoPlay)
        {
            if (Random.value > missChance)
            {
                // Simulate perfect timing or slight offset
                float delay = Random.value > perfectTimingChance ? Random.Range(0.05f, 0.15f) : 0f;
                Invoke("SimulateParry", delay);
            }
        }
    }

    void SimulateParry()
    {
        // Simulate parry input
        if (player != null)
        {
            SendMessage("TryParry", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnGUI()
    {
        if (!showDebugInfo) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        GUILayout.Label("--- RHYTHM GAME DEBUG ---");

        if (beatDetector != null)
        {
            GUILayout.Label("BPM: " + beatDetector.bpm);
            GUILayout.Label("Beat Threshold: " + beatDetector.beatThreshold);
            GUILayout.Label("Beat Offset: " + beatDetector.GetBeatOffset().ToString("F3"));
            GUILayout.Label("On Beat: " + (beatDetector.IsOnBeat() ? "YES" : "NO"));
        }

        if (player != null)
        {
            GUILayout.Label("Player Health: " + player.currentHealth + "/" + player.maxHealth);
        }

        GUILayout.Label("Last Beat Time: " + lastBeatTime.ToString("F2"));

        GUILayout.EndArea();
    }
}