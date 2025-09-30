using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    Rigidbody rigidBody;
    Collider col;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        col.isTrigger = false;
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
    }
}
