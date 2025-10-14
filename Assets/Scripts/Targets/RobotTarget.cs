using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotTarget : MonoBehaviour, IShootable
{
    [Header("Dead State")]
    public Vector3 positionTransform = new Vector3(0, -0.3f, 1);
    public Quaternion rotationTransform = Quaternion.Euler(90, 10, 0);

    public float respawnTime = 2;
    public float killDuration = 0.02f;
    public float respawnDuration = 0.3f;

    [Header("Score")]
    public float points = 1f;


    [Header("Movement")]
    public Transform waypointsParent;
    public float waitTime = 0f;

    private Transform[] waypoints;
    private int currentIndex = 0;

    private NavMeshAgent agent;
    private float waitTimer = 0f;


    private bool unconcious = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new Transform[waypointsParent.childCount];
        for (int i = 0; i < waypointsParent.childCount; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i);
        }

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        // Check if reached current waypoint
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (waitTimer <= 0f)
            {
                // Set next waypoint
                currentIndex = (currentIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentIndex].position);
                waitTimer = waitTime;
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }
    }


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

        agent.isStopped = true;


        var startPos = transform.localPosition;
        var startRot = transform.localRotation;
        var endPos = startPos + positionTransform;
        var endRot = startRot * rotationTransform;

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

        //anim.enabled = true; // stops all animation
        agent.isStopped = false;

        // update state
        unconcious = false;
    }
}
