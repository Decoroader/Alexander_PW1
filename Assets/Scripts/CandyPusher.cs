using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    private Rigidbody currentRigid;
    private float speed = 3;

    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
		if (Input.GetMouseButtonUp(0))
			currentRigid.AddForce(Vector3.forward * speed, ForceMode.Impulse);
	}
}
