using UnityEngine;
using UnityEngine.UI;

public class BeatVisualizer : MonoBehaviour
{
    public Image beatIndicator;
    public Image perfectBeatIndicator;
    public Image playerInputIndicator;

    public float indicatorSize = 100f;
    public Color perfectBeatColor = Color.green;
    public Color missedBeatColor = Color.red;

    private BeatDetector beatDetector;

    void Start()
    {
        beatDetector = FindObjectOfType<BeatDetector>();

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
        // Show beat indicator
        ShowBeatIndicator();
    }

    void ShowBeatIndicator()
    {
        if (beatIndicator != null)
        {
            // Visual pulse effect
            beatIndicator.transform.localScale = Vector3.one * 1.5f;
            LeanTween.scale(beatIndicator.gameObject, Vector3.one, 0.2f);
            LeanTween.color(beatIndicator.rectTransform, Color.white, 0.2f);
        }

        if (perfectBeatIndicator != null)
        {
            // Highlight perfect timing window
            perfectBeatIndicator.color = perfectBeatColor;
            LeanTween.color(perfectBeatIndicator.rectTransform, new Color(perfectBeatColor.r, perfectBeatColor.g, perfectBeatColor.b, 0), 0.2f);
        }
    }

    void Update()
    {
        // Update beat indicator position based on current beat position
        if (beatDetector != null)
        {
            float beatOffset = beatDetector.GetBeatOffset();

            // Visualize the beat position
            // This is a simplified example - you might want to create a more sophisticated visualization
            if (beatIndicator != null)
            {
                beatIndicator.fillAmount = Mathf.Abs(beatOffset) / 0.5f;
            }
        }

        // Visualize player input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            ShowPlayerInput();
        }
    }

    void ShowPlayerInput()
    {
        if (playerInputIndicator != null)
        {
            // Determine if the input was on beat
            bool onBeat = beatDetector.IsOnBeat();

            // Visualize input timing
            playerInputIndicator.color = onBeat ? perfectBeatColor : missedBeatColor;
            LeanTween.color(playerInputIndicator.rectTransform, new Color(playerInputIndicator.color.r, playerInputIndicator.color.g, playerInputIndicator.color.b, 0), 0.5f);
        }
    }
}