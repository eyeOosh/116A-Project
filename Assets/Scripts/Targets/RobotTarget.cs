using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RobotTarget : MonoBehaviour, IShootable
{
    [Header("Unconciousnes")]
    public Vector3 positionTransform = new Vector3(0, -0.3f, 1);
    public Quaternion rotationTransform = Quaternion.Euler(90, 10, 0);

    public float respawnTime = 2;
    public float killDuration = 0.02f;
    public float respawnDuration = 0.3f;

    [Header("Score")]
    public float points = 1f;


    private bool unconcious = false;

    public bool Hit(float damage)
    {
        if (unconcious)
            return false;

        unconcious = true;
        StartCoroutine(HitRoutine());
        return true;
    }

    private IEnumerator HitRoutine()
    {
        // add score
        ScoreManager.Instance?.AddScore(points);

        var startPos = transform.localPosition;
        var startRot = transform.localRotation;
        var endPos = startPos + positionTransform;
        var endRot = startRot * rotationTransform;

        // TODO: trigger hit animation as well

        // kill animation
        float elapsed = 0f;
        while (elapsed < killDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / killDuration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(respawnTime);

        // respawn animation
        elapsed = 0f;
        while (elapsed < respawnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / respawnDuration;
            transform.localPosition = Vector3.Lerp(endPos, startPos, t);
            transform.localRotation = Quaternion.Slerp(endRot, startRot, t);
            yield return null;
        }

        // update state
        unconcious = false;
    }
}
