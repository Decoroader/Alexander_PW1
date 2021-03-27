using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenedCandyControl : MonoBehaviour
{
    private float speedOnOpenSpace = 3.9f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision toHead_)
    {
        if (toHead_.gameObject.CompareTag("Head"))           // destroy the opened candy in the head
            Destroy(gameObject, 0.19f);
    }
}
