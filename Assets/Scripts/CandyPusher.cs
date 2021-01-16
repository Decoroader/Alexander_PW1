using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public GameObject prefabTransit;
    private GameObject currentTransit;
    private float speedOnOpenSpace = 5.5f;
    private float speedInTube = 7;
    private float hightBound = 17;

    private Rigidbody currentRigid;
    private Quaternion initRotation;

    private int countForMoveInTube = 0;
    private int countWaitToReceiver = 39;
    private int scaleTransformer = 3;
    private int speedGame = 199;

    private string receiverString = "Receiver";

    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        initRotation = transform.rotation;
        StartCoroutine(WaitForPlayerDecision());
    }

    void Update()
    {
        if (countForMoveInTube-- > 0)       // move the candy in the tube
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);

        if ((transform.position.z > hightBound) || (transform.position.x > hightBound) || (transform.position.x < -hightBound))     
            Destroy(gameObject);            // destroy the candy out of he bounds
    }
    private void OnCollisionEnter(Collision toHead)
	{
        if (toHead.gameObject.CompareTag("Head"))           // destroy the candy in the head
            Destroy(gameObject, 0.19f);
        if (toHead.gameObject.CompareTag("DinamicObject"))  // collision with other candy
            currentRigid.useGravity = true;                 // the other candy falls and out from the game
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Spawner")
            currentRigid.useGravity = true;                 // if the candy not hit the receiver

        if (countForMoveInTube < 99)                        // this check is necessary because OnTriggerEnter is called multiple times
        {
            if (other.gameObject.CompareTag(receiverString))
            {
                StopVelocity();
                StopAllCoroutines();
                currentRigid.useGravity = false;                // the candy have hit the receiver

                transform.position = other.gameObject.transform.position;    // to get form the current receiverposition
                transform.rotation = other.gameObject.transform.rotation;    // to set from the current receiverrotation
                transform.localScale /= scaleTransformer;       // reduce scale to hide the candy in the transit sphere
                
                countForMoveInTube = 111;
                currentTransit = Instantiate(prefabTransit, transform.position, transform.rotation) as GameObject;
                currentTransit.transform.SetParent(transform);
            }
        }
        
        if (other.gameObject.CompareTag("OutSphere"))
		{
            StopVelocity();
            Destroy(currentTransit);

            transform.rotation = initRotation;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            if(transform.localScale.x < 1)
                transform.localScale *= scaleTransformer;         // back the scale to show the candy in the track to the head

            //countForMoveToHead = 77;
            currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); // move the candy to the head side

            countForMoveInTube = 0;
        }
    }

    IEnumerator WaitForPlayerDecision()
    {
        bool isPressedMouseButton = false;
        while (speedGame-- > 0)                         // Wait For the Players Decision
        {
            if (Input.GetMouseButtonDown(0))            // 
                isPressedMouseButton = true;
            if (isPressedMouseButton)
            {
                if (Input.GetMouseButtonUp(0))              // runs the cundy in the receiver side?
                {
                    currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse);
                    // move the candy to the receiverside
                    StartCoroutine(WaitForReciever());      // wait for hit the receiver
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        currentRigid.useGravity = true;         // the candy falls and out from the game
        yield return null;
    }
	IEnumerator WaitForReciever()
	{
		while (true)
		{
			if (!(countWaitToReceiver-- > 0))           // while move the candy to the receiverside
			{
				currentRigid.useGravity = true;         // the candy falls and out from the game since missed receiver
				break;
			}
			yield return new WaitForFixedUpdate();
		}
	}
	void StopVelocity()
	{
		currentRigid.velocity = Vector3.zero;
		currentRigid.angularVelocity = Vector3.zero;
	}
}
