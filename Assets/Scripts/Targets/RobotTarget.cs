using UnityEngine;
using System.Collections;

public class RobotTarget : MonoBehaviour, IShootable
{
    [Header("Score")]
    public float score = 1f;


    public void Hit(float damage)
    {
        ScoreManager.Instance?.AddScore(score);

        Debug.LogWarning($"{nameof(RobotTarget)} hit");
        
        // TODO: add animations, health etc...
    }
}
