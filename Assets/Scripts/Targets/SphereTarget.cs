using UnityEngine;
using System.Collections;

public class SphereTarget : MonoBehaviour, IShootable
{
    [Header("Box Health")]
    public float maxHealth = 3f;
    private float currentHealth;

    [Header("Respawn Settings")]
    public float respawnDelay = 0.02f;
    public float minX = -3f, maxX = 1.3f;
    public float minY = -0.5f, maxY = 2.5f;
    public float minZ = 0f, maxZ = 0f;

    [Header("Spawn Collision Settings")]
    public float safeRadius = 0.5f;   // Minimum distance from another target
    public int maxAttempts = 20;      // Number of tries before giving up

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
        currentHealth = maxHealth;
        anchorPosition = transform.position;
        timeOffset = Random.value * 10f; 
    }

    void Update()
    {
        if (!enableMovement) return;

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

    public bool Hit(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ScoreManager.Instance?.AddScore(1f); // TODO: make score dynamic
            StartCoroutine(Respawn());
        }

        return true;
    }

    private IEnumerator Respawn()
    {
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);

        Vector3 newPos = FindSafePosition();

        // Reset anchor so movement centers on new spawn
        anchorPosition = newPos;
        transform.position = newPos;

        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }

    private Vector3 FindSafePosition()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                Random.Range(minZ, maxZ)
            );

            if (IsPositionSafe(candidate))
                return candidate;
        }
        return transform.position; // fallback
    }

    private bool IsPositionSafe(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, safeRadius);
        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject && col.CompareTag("Target"))
            {
                return false;
            }
        }
        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), Mathf.Abs(maxZ - minZ));
        Gizmos.DrawCube(center, size);

        // Draw the movement path preview
        if (!Application.isPlaying)
        {
            anchorPosition = transform.position;
        }

        // Direction vector for gizmo
        Vector3 dir = moveAxis == Axis.X ? Vector3.right :
                      moveAxis == Axis.Y ? Vector3.up    :
                                           Vector3.forward;

        Vector3 a = anchorPosition - dir * (moveDistance * 0.5f);
        Vector3 b = anchorPosition + dir * (moveDistance * 0.5f);

        Gizmos.color = new Color(0f, 0.6f, 1f, 0.8f);
        Gizmos.DrawLine(a, b);
        Gizmos.DrawSphere(a, 0.05f);
        Gizmos.DrawSphere(b, 0.05f);
    }
}
