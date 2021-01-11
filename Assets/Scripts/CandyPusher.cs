using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public GameObject prefabTransit;
    private GameObject currentTransit;
    private float speedOnOpenSpace = 3;
    private float speedInTube = 7;
    private float hightBound = 17;

    private Rigidbody currentRigid;
    private Quaternion initRotation;

    private int countForMoveInTube;
    private int countForMoveToHead;
    private int countForMoveToReciever = 77;
    private int speedGame;

    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        initRotation = transform.rotation;
        speedGame = 199;
        StartCoroutine(WaitForDecision());
    }

    void Update()
    {
        if (countForMoveInTube-- > 0)       // move the candy in the tube
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);

        if (countForMoveToHead-- > 0)       // move the candy to the head side
            transform.Translate(Vector3.forward * speedOnOpenSpace * Time.deltaTime);

        if ((transform.position.z > hightBound) || (transform.position.x > hightBound) || (transform.position.x < -hightBound))     
            Destroy(gameObject);            // destroy the candy out of he bounds
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
            StopAllCoroutines();

            transform.position = other.gameObject.transform.position;    // to get form the current reciever position
            transform.rotation = other.gameObject.transform.rotation;    // to set from the current reciever rotation
            transform.localScale *= 0.1f;       // reduce scale to hide the candy in the transit sphere

            countForMoveInTube = 111;
            currentTransit = Instantiate(prefabTransit, transform.position, transform.rotation) as GameObject;
            currentTransit.transform.SetParent(transform);
        }
        if (other.gameObject.CompareTag("OutSphere"))
		{
            Destroy(currentTransit);

            transform.rotation = initRotation;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            transform.localScale *= 10;         // back the scale to show the candy in the track to the head

            countForMoveToHead = 77;
            countForMoveInTube = 0;
        }
    }

    IEnumerator WaitForDecision()
    {
        int waitCount = 0;                  // timer for wait player's decision

        while (true)
        {
            if (Input.GetMouseButtonUp(0))              // the cundy runs in the reciever side
            {
                StartCoroutine(WaitForReciever());
                break;
            }

            if (waitCount++ > speedGame)
            {
                currentRigid.useGravity = true;         // the candy falls and out from game
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator WaitForReciever() {
        while (true)
        {
            if (countForMoveToReciever-- > 0)           // move the candy to the reciever side
                transform.Translate(Vector3.forward * speedOnOpenSpace * Time.deltaTime);
			else
			{
                currentRigid.useGravity = true;         // the candy falls and out from game
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
 //   void StopVelocity()
	//{
	//	currentRigid.velocity = Vector3.zero;
	//	currentRigid.angularVelocity = Vector3.zero;
	//}
}
