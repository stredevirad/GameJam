using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void Initialize(int level)
    {
        health += level * 10; // Increase health based on level
    }
}