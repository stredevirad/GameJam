using UnityEngine;

using UnityEngine.UI;

using System.Collections;
using System;



public class UIManager : MonoBehaviour

{

    public Image beatVisualizer;

    public Slider healthBar; // Reference to the UI health bar



    public void UpdateBeatVisualizer()

    {

        if (beatVisualizer != null)

        {

            StartCoroutine(BeatPulse());

        }

        else

        {

            Debug.LogWarning("BeatVisualizer is not assigned in UIManager!");

        }

    }



    private IEnumerator BeatPulse()

    {

        beatVisualizer.color = Color.red; // Flash effect

        yield return new WaitForSeconds(0.1f);

        beatVisualizer.color = Color.white;

    }



    public void UpdateHealthBar(int health)

    {

        if (healthBar != null)

        {

            healthBar.value = health;

        }

        else

        {

            Debug.LogWarning("HealthBar UI element is not assigned in UIManager!");

        }

    }

    internal void UpdateHealthBar(object health)
    {
        throw new NotImplementedException();
    }
}