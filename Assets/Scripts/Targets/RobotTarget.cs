using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RobotTarget : MonoBehaviour, IShootable
{
    [Header("Dead State")]
    public Vector3 positionTransform = new Vector3(0, -0.3f, 0.35f);
    public Quaternion rotationTransform = Quaternion.Euler(90, 0, 0);

    public float respawnTime = 2;
    public float dieDuration = 0.15f;
    public float respawnDuration = 0.5f;

    [Header("Scoring")]
    public float points = 1f;

    [Header("Movement")]
    public Transform waypointsParent;

    // private state
    private Transform[] waypoints;
    private int currentIndex = 0;

    private NavMeshAgent agent;
    private Transform robot;
    private Animation animator;

    private bool dead = false;

    public float health = 100f;
    public AudioSource hitAudio;
    public AudioClip hitSound;


    void Start()
    {
        // find referenced components
        robot = transform;
        if (robot == null)
        Debug.LogError("[RobotTarget] Could not find child Transform named 'Robot'.");

        animator = transform.Find("Robot").GetComponent<Animation>();

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        Debug.LogError("[RobotTarget] No NavMeshAgent found on this GameObject.");


        // create waypoints
        waypoints = new Transform[waypointsParent.childCount];
        for (int i = 0; i < waypointsParent.childCount; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i);
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
        if (hitAudio == null) hitAudio = GetComponent<AudioSource>();
        if (hitAudio != null) { hitAudio.playOnAwake = false; hitAudio.loop = false; hitAudio.clip = null; }

    }

    void Update()
    {

        // Bail out if movement isn’t properly set up
    if (agent == null || waypoints == null || waypoints.Length == 0)
        return;

    // Make sure the next waypoint exists
    if (!agent.pathPending && agent.remainingDistance < 0.1f)
    {
        currentIndex = (currentIndex + 1) % waypoints.Length;

        var next = waypoints[currentIndex];
        if (next == null)
        {
            Debug.LogWarning($"[RobotTarget] Waypoint at index {currentIndex} is null.");
            return;
        }

        agent.SetDestination(next.position);
    }
        // update waypoint
        // if (!agent.pathPending && agent.remainingDistance < 0.1f)
        // {
        //     currentIndex = (currentIndex + 1) % waypoints.Length;
        //     agent.SetDestination(waypoints[currentIndex].position);
        // }


    }

    public bool Hit(float damage)
    {
        if (dead)
            return false;

        dead = true;

        StartCoroutine(HitRoutine());
        Debug.Log($"Robot TakeDamage({damage})"); // must print every time you shoot it

        if (hitAudio != null && hitSound != null)
        {
            hitAudio.PlayOneShot(hitSound);
        }

        return true;
    }

    private IEnumerator HitRoutine()
    {
        ScoreManager.Instance?.AddScore(points);
        agent.isStopped = true;
        animator.Stop();


        // move down
        var startPos = robot.localPosition;
        var startRot = robot.localRotation;
        var endPos = startPos + positionTransform;
        var endRot = startRot * rotationTransform;

        // kill animation
        float elapsed = 0f;
        while (elapsed < dieDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dieDuration;
            robot.localPosition = Vector3.Lerp(startPos, endPos, t);
            robot.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(respawnTime);

        // respawn animation
        elapsed = 0f;
        while (elapsed < respawnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / respawnDuration;
            robot.localPosition = Vector3.Lerp(endPos, startPos, t);
            robot.localRotation = Quaternion.Slerp(endRot, startRot, t);
            yield return null;
        }

        animator.Play();
        agent.isStopped = false;
        dead = false;
    }
}
