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

    [Header("Settings")]
    // range in units of the ray
    public float range = 100f;
    // damage per hit
    public float damage = 2f;
    // fire rate in shots per second
    public float fireRate = 50f; // shots per second
    // public float nextTimeToFire = 0f; // time when player can fire next
    public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);


    private float nextFire = 0f;
    public Transform gunEnd; // assign this in the Inspector

   

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();

        // gunAudio = GetComponent<AudioSource>();

        fpsCam = GetComponentInParent<Camera>();
    }



    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time > nextFire) 
        {
            nextFire = Time.time + 1f / fireRate;

            StartCoroutine (ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            laserLine.SetPosition (0, gunEnd.position);

            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, range))
            {
                laserLine.SetPosition (1, hit.point);

                ShootableBox health = hit.collider.GetComponent<ShootableBox>();

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
    // gunAudio.Play ();

    laserLine.enabled = true;

    yield return shotDuration;

    laserLine.enabled = false;
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
