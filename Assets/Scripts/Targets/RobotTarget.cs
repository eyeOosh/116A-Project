using UnityEngine;
using System.Collections;

public class RobotTarget : MonoBehaviour, IShootable
{
    [Header("Unconciousnes")]
    public Vector3 positionTransform = new Vector3(0, -0.3f, 1);
    public Quaternion rotationTransform = Quaternion.Euler(90, 10, 0);
    public int respawnTime = 5;

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

        // animate unconciousnes
        transform.localPosition += positionTransform;
        transform.localRotation *= rotationTransform;

        yield return new WaitForSeconds(respawnTime);

        transform.localPosition -= positionTransform;
        transform.rotation *= Quaternion.Inverse(rotationTransform);

        // update state
        unconcious = false;
    }

}
