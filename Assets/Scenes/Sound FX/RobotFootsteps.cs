using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class RobotFootsteps : MonoBehaviour
{
    [Header("References")]
    public AudioSource RobotFootstepsSound;  
    public Transform Player;                 

    [Header("Movement Detection")]
    [Tooltip("Minimum speed (m/s) that counts as moving")]
    public float movingSpeedThreshold = 0.1f;

    [Header("Proximity Volume Settings")]
    public float minDistance = 1f;           
    public float maxDistance = 20f;          
    [Range(0f,1f)] public float maxVolume = 1f;
    public float volumeSmooth = 10f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Vector3 lastPos;

    void Awake()
    {
        if (!RobotFootstepsSound) RobotFootstepsSound = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        rb    = GetComponent<Rigidbody>();
        lastPos = transform.position;

    }

    void Update()
    {
        bool isMoving = IsActuallyMoving();

        if (isMoving)
        {
            if (!RobotFootstepsSound.isPlaying) RobotFootstepsSound.Play();
        }
        else
        {
            if (RobotFootstepsSound.isPlaying)  RobotFootstepsSound.Stop();
        }

        if (Player)
        {
            float distance = Vector3.Distance(Player.position, transform.position);
            float t = Mathf.InverseLerp(maxDistance, minDistance, distance); // 0..1 near
            t = Mathf.SmoothStep(0f, 1f, t);
            float targetVol = maxVolume * t;
            RobotFootstepsSound.volume = Mathf.Lerp(RobotFootstepsSound.volume, targetVol, Time.deltaTime * volumeSmooth);
        }

        lastPos = transform.position;
    }

    bool IsActuallyMoving()
    {
        float thr2 = movingSpeedThreshold * movingSpeedThreshold;

        if (agent)
        {
            bool intent = agent.hasPath && !agent.isStopped && agent.remainingDistance > agent.stoppingDistance + 0.01f;
            Vector3 v = agent.velocity.sqrMagnitude > 0.0001f ? agent.velocity : agent.desiredVelocity;
            return intent && v.sqrMagnitude > thr2;
        }

        if (rb)
        {
            return rb.linearVelocity.sqrMagnitude > thr2;
        }

        Vector3 delta = (transform.position - lastPos);
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-6f);
        return speed * speed > thr2;
    }
}
