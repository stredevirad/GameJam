using UnityEngine;

public class DifficultyBalancer : MonoBehaviour
{
    [Header("Difficulty Scaling")]
    public AnimationCurve healthScaling;
    public AnimationCurve damageScaling;
    public AnimationCurve bpmScaling;

    [Header("Level Settings")]
    public int maxLevel = 10;

    private