using UnityEngine;
using UnityEngine.InputSystem; // new Input System

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;

    [Header("Settings")]
    public float range = 100f;
    public float damage = 10f;
    public float fireRate = 5f; // shots per second

    private float nextTimeToFire = 0f;
    public Transform muzzleTip; // assign this in the Inspector


    void Update()
    {
        // Semi-auto: fires once per click
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        Ray ray = new Ray(muzzleTip.position, muzzleTip.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red); // default duration = 0, lasts 1 frame
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            TargetBehavior target = hit.transform.GetComponent<TargetBehavior>();
            if (target != null)
            {
                target.Hit();
            }

            if (hitEffect != null)
            {
                GameObject impactGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

    }
}
