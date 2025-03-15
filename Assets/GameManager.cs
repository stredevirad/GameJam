using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 1;
    public int enemiesToDefeat = 5;
    public AudioManager audioManager;

    void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        audioManager.PlayLevelMusic(currentLevel);
    }

    public void EnemyDefeated()
    {
        enemiesToDefeat--;
        if (enemiesToDefeat <= 0)
        {
            Debug.Log("Level Complete!");
            // Implement level transition logic here
        }
    }

    public void PlayerDamaged()
    {
        Debug.Log("Player took damage!");
        // Implement player damage logic
    }
}