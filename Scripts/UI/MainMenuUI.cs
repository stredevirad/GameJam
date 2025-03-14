using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject levelSelectPanel;

    [Header("Options")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    void Start()
    {
        // Show main panel
        ShowPanel(mainPanel);

        // Initialize options
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    void ShowPanel(GameObject panel)
    {
        // Hide all panels
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);

        // Show the selected panel
        if (panel != null) panel.SetActive(true);
    }

    // Button callbacks
    public void OnPlayButton()
    {
        // Load the first level
        SceneManager.LoadScene("Level1");
    }

    public void OnOptionsButton()
    {
        ShowPanel(optionsPanel);
    }

    public void OnCreditsButton()
    {
        ShowPanel(creditsPanel);
    }

    public void OnLevelSelectButton()
    {
        ShowPanel(levelSelectPanel);
    }

    public void OnBackButton()
    {
        ShowPanel(mainPanel);
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Options callbacks
    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Level select callbacks
    public void OnLevelButton(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }
}