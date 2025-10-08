using UnityEngine;
using System.Collections;

public class ShootableBox : MonoBehaviour
{
    public float currentHealth = 3;

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            // gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }
    
}
