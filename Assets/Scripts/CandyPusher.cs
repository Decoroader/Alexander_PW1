using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    private Rigidbody currentRigid;
    private float speedOnOpenSpace = 3;
    private float speedInTube = 7;
    private float hightBound = 17;

    private int speedGame;

    //private Vector3 startFlyToHeadPos;
    private Vector3 startMoveInTubePos;
    private Quaternion startMoveInTubeRot;
    private Quaternion initRotation;

    private int count;
    
    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        speedGame = 199;
        initRotation = transform.rotation;
        //startFlyToHeadPos = new Vector3(0, 1, tubeDistance);
        StartCoroutine(WaitForDecision());
    }

    void Update()
    {
		if (Input.GetMouseButtonUp(0))                      // the cundy runs in the head side
			currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); 

        if ((transform.position.z > hightBound) || (transform.position.x > hightBound))     // destroy the candy out of he bounds
            Destroy(gameObject);

        if (count-- > 0)
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);
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
            currentRigid.velocity = Vector3.zero;
            currentRigid.angularVelocity = Vector3.zero;

            transform.position = other.gameObject.transform.position;    // to get form the current reciever position
            transform.rotation = other.gameObject.transform.rotation;    // to set from the current reciever rotation
            transform.localScale *= 0.1f;       // reduce scale to hide the candy in the transit sphere
            StopAllCoroutines();                // stop waiting for the decision

            //Vector3 tV3 = new Vector3(0, 1, 9.0f) - transform.position;
            //currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse);
            //currentRigid.AddForce(tV3 * (speedDinamicObject - 1), ForceMode.Impulse);
            count = 100;
            //Debug.Log("position = " + transform.position);

        }
        if (other.gameObject.CompareTag("OutSphere"))
		{
            StopAllCoroutines();                // stop Tube Transit
            transform.localScale *= 10;         // back the scale to show the candy in the track to the head
            transform.rotation = initRotation;
            currentRigid.velocity = Vector3.zero;
            currentRigid.angularVelocity = Vector3.zero;

            currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse);
            //Debug.Log("localScale = " + transform.localScale);
            count = 0;
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
}
