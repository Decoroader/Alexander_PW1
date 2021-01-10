using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    private Rigidbody currentRigid;
    private float speedOnOpenSpace = 3;
    private float speedInTube = 7;
    private float hightBound = 17;

    private int speedGame;

    private Quaternion initRotation;

    private int countForMoveInTube;
    
    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        speedGame = 199;
        initRotation = transform.rotation;
        StartCoroutine(WaitForDecision());
    }

    void Update()
    {
		if (Input.GetMouseButtonUp(0))                      // the cundy runs in the head side
			currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); 

        if (countForMoveInTube-- > 0)
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);

        if ((transform.position.z > hightBound) || (transform.position.x > hightBound))     // destroy the candy out of he bounds
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision toHead)
	{
        if (toHead.gameObject.CompareTag("Head"))   // destroy the candy in the head
            Destroy(gameObject, 0.33f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reciever"))
        {
            StopVelocity();
            StopAllCoroutines();                // stop waiting for the decision

            transform.position = other.gameObject.transform.position;    // to get form the current reciever position
            transform.rotation = other.gameObject.transform.rotation;    // to set from the current reciever rotation
            transform.localScale *= 0.1f;       // reduce scale to hide the candy in the transit sphere

            countForMoveInTube = 111;
        }
        if (other.gameObject.CompareTag("OutSphere"))
		{
            StopVelocity();

            transform.rotation = initRotation;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            transform.localScale *= 10;         // back the scale to show the candy in the track to the head

            currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse);
            countForMoveInTube = 0;
        }
    }
    IEnumerator WaitForDecision()
    {
        int waitCount = 0;                  // timer for wait player's decision
        while (waitCount < speedGame)
        {
            yield return new WaitForFixedUpdate();
            waitCount++;
        }
        yield return null;
        currentRigid.useGravity = true;     // the candy falls and out from game
    }
    void StopVelocity()
	{
        currentRigid.velocity = Vector3.zero;
        currentRigid.angularVelocity = Vector3.zero;
    }
}
