using UnityEngine;
using System.Collections;

public class ShootableBox : MonoBehaviour
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
    public int maxAttempts = 20;    // Number of tries before giving up

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);

        Vector3 newPos = FindSafePosition();

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

        Debug.LogWarning($"{name} could not find a safe position after {maxAttempts} attempts.");
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
    }
}
