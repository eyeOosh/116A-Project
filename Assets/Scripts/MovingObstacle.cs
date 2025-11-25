using UnityEngine;
using System.Collections;

public class MovingObstacle : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────────
    // Oscillating movement settings
    // ─────────────────────────────────────────────────────────────────────────────
    public enum Axis { X, Y, Z }

    [Header("Oscillating Movement")]
    public bool enableMovement = true;
    public Axis moveAxis = Axis.X;     // Which axis to move along
    public float moveDistance = 2f;    // Total travel distance end-to-end
    public float moveSpeed = 1.5f;     // Cycles per second (higher = faster)
    public bool useLocalSpace = false; // If true, moves along local axis

    private Vector3 anchorPosition;    // Center point of oscillation
    private float timeOffset;          // Randomized so instances aren't in sync

    void Start()
    {
        anchorPosition = transform.position;
        timeOffset = Random.value * 10f; // desync multiple targets a bit
    }

    void Update()
    {
        if (!enableMovement) return;

        // PingPong gives 0..moveDistance. Shift to be centered around anchor.
        float t = Mathf.PingPong((Time.time + timeOffset) * moveSpeed, moveDistance) - (moveDistance * 0.5f);

        Vector3 dir;
        if (useLocalSpace)
        {
            // local right/up/forward
            dir = moveAxis == Axis.X ? transform.right :
                  moveAxis == Axis.Y ? transform.up    :
                                       transform.forward;
        }
        else
        {
            // world axes
            dir = moveAxis == Axis.X ? Vector3.right :
                  moveAxis == Axis.Y ? Vector3.up    :
                                       Vector3.forward;
        }

        transform.position = anchorPosition + dir * t;
    }

}
