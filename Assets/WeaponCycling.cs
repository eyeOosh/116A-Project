using UnityEngine;
using System.Collections;

public class WeaponCycling : MonoBehaviour
{
    [Header("Weapon List")]
    public GameObject[] weapons;
    public Transform spawnWeapon;

    [Header("Weapon Behavior")]
    public float switchInt = 2f;
    public float rotation = 45f;

    private int current = -1;
    private GameObject currentInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnWeapon == null)
        {
            spawnWeapon = transform;
        }

        StartCoroutine(Cycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentInstance != null)
        {
            currentInstance.transform.Rotate(0f, rotation * Time.deltaTime, 0f, Space.World);
        }
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            NextWeapon();
            yield return new WaitForSeconds(switchInt);
        }
    }

    private void NextWeapon()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        current = (current + 1)%weapons.Length;

        currentInstance = Instantiate(weapons[current], spawnWeapon.position, spawnWeapon.rotation, spawnWeapon);
    }
}
