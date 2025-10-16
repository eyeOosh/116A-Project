using UnityEngine;
using UnityEngine.InputSystem; // new Input System
using System.Collections;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    // private AudioSource gunAudio;
    public LineRenderer laserLine;
    public AudioSource gunAudio;
    public AudioClip fireSound;
    public AudioSource robotAudioWhenShot;
    public AudioClip tinSound;

    [Header("Settings")]
    public float range = 100f;
    public float damage = 2f;
    public float fireRate = 5f; // shots per second
    public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);


    private float nextFire = 0f;
    public Transform gunEnd; // assign this in the Inspector

   

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();

        gunAudio = GetComponent<AudioSource>();

        robotAudioWhenShot = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
        // ensure no auto-play at startup
    if (gunAudio != null)
    {
        gunAudio.playOnAwake = false;
        gunAudio.loop = false;
        gunAudio.clip = null; // keep the clip only in fireSound
    }
    if (robotAudioWhenShot != null)
    {
        robotAudioWhenShot.playOnAwake = false;
        robotAudioWhenShot.loop = false;
        robotAudioWhenShot.clip = null; // keep the clip only in fireSound
    }

        
    }



    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time > nextFire) 
        {
            nextFire = Time.time + 1f / fireRate;

            StartCoroutine (ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

<<<<<<< Updated upstream
            laserLine.SetPosition (0, gunEnd.position);

            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, range))
=======

            
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out RaycastHit hit, range))
>>>>>>> Stashed changes
            {
                laserLine.SetPosition (1, hit.point);

<<<<<<< Updated upstream
                ShootableBox health = hit.collider.GetComponent<ShootableBox>();
=======
                // invoke shootable interface
                IShootable shootable = hit.collider.GetComponentInParent<IShootable>();
                if(shootable is not null)
                {
                    var isHit = shootable.Hit(damage);
                    robotAudioWhenShot.PlayOneShot(tinSound);

>>>>>>> Stashed changes

                if (health != null)
                {
                    health.Damage(damage); //convert to float

                }

                if (hit.rigidbody != null);
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);

                }
            }
            else
            {
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * range));
            }
        }
        // Semi-auto: fires once per click
        // if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextTimeToFire)
        // {
        //     nextTimeToFire = Time.time + 1f / fireRate;
        //     Shoot();
        // }

        // Ray ray = new Ray(muzzleTip.position, muzzleTip.forward);
        // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red); // default duration = 0, lasts 1 frame
    }

    private IEnumerator ShotEffect()
    {
<<<<<<< Updated upstream
    // gunAudio.Play ();
=======
        gunAudio.PlayOneShot(fireSound);
>>>>>>> Stashed changes

    laserLine.enabled = true;

    yield return shotDuration;

<<<<<<< Updated upstream
    laserLine.enabled = false;
=======
        laserLine.enabled = false;
        
>>>>>>> Stashed changes
    }

    // void Shoot()
    // {
    //     if (muzzleFlash != null)
    //         muzzleFlash.Play();

    //     Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    //     RaycastHit hit;

    //     if (Physics.Raycast(ray, out hit, range))
    //     {
    //         Debug.Log("Hit: " + hit.transform.name);

    //         TargetBehavior target = hit.transform.GetComponent<TargetBehavior>();
    //         if (target != null)
    //         {
    //             target.Hit();
    //         }

    //         if (hitEffect != null)
    //         {
    //             GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
    //             Destroy(impactGO, 2f);
    //         }
    //     }

    //     Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

    // }
}
