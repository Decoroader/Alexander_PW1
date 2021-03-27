using System.Collections;
using UnityEngine;

public class CandyReceiver_OutSphere : MonoBehaviour
{
    public GameObject prefabTransit;
    public GameObject[] prefabsOpenCandy;
    public AudioClip notSound;

    private AudioSource playerAudio;
    private GameObject currentTransit;
    private float speedToHead = 5.9f;
    private float speedInTube = 7;

    private Rigidbody currentRigid;

    private Vector3 initPosition;
    private Quaternion initRotation;
    private int countForMoveInTube = 0;
    private int countWaitToReceiver = 39;
    private int scaleTransformer = 3;
    private Vector3 tempLocalScale;

    private string receiverString = "Receiver";
    private int currentIndex;

    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        initRotation = transform.rotation;
        initPosition = transform.position;
        currentIndex = ObjectColorIndex.GetCurrentObjectIndex(gameObject);
        StartCoroutine(WaitForReciever());
        playerAudio  = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (countForMoveInTube-- > 0)       // move the candy in the tube
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision candy)
	{
        if (candy.gameObject.CompareTag("DinamicObject")|| candy.gameObject.CompareTag("ODinamicObject"))
            currentRigid.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (countForMoveInTube < 99)                        // this check is necessary because OnTriggerEnter is called multiple times
        {
            if (other.gameObject.CompareTag(receiverString))
            {
                if ((int)char.GetNumericValue(other.gameObject.name[other.gameObject.name.Length - 1]) ==
                    currentIndex)
                {
                    StopVelocity();
                    StopAllCoroutines();
                    currentRigid.useGravity = false;                // the candy have hit the receiver

                    transform.position = other.gameObject.transform.position;    // to set from the current receiverposition
                    transform.rotation = other.gameObject.transform.rotation;    // to set from the current receiverrotation
                    tempLocalScale = transform.localScale;
                    transform.localScale /= scaleTransformer;       // reduce scale to hide the candy in the transit sphere

                    countForMoveInTube = 111;
                    currentTransit = Instantiate(prefabTransit, transform.position, transform.rotation) as GameObject;
                    currentTransit.transform.SetParent(transform);
                }
                else
                {
                    playerAudio.PlayOneShot(notSound, 1.0f);      // call not sound, as candy collided to incorrect receiver
                    currentRigid.velocity *= -0.33f;
                }
            }
        }
        
        if (other.gameObject.CompareTag("OutSphere"))
		{
            StopVelocity();
            Destroy(currentTransit);
            transform.position = new Vector3(0, 1.1f, transform.position.z);

            if (gameObject.CompareTag("ODinamicObject"))
            {
                Instantiate(prefabsOpenCandy[currentIndex],
                    gameObject.transform.position, prefabsOpenCandy[currentIndex].transform.rotation); // opened candy insta 
                gameObject.transform.position = Vector3.forward * (-11); // its hiding the main candy behind
                Destroy(gameObject, 1);
            }
            else
            {
                transform.rotation = initRotation;
                transform.localScale = tempLocalScale;         // back the scale to show the candy in the track to the head
                currentRigid.AddForce(Vector3.forward * speedToHead, ForceMode.Impulse);
            }
            // move the candy to the head side

            countForMoveInTube = 0;
        }
    }
	IEnumerator WaitForReciever()
	{
		while (true)
		{
            if (initPosition.z != transform.position.z)
            {
                if (!(countWaitToReceiver-- > 0))           // while candy moving to the receiverside
                    currentRigid.useGravity = true;         // the candy falls and out from the game since missed receiver
            }
            if (currentRigid.useGravity)
                break;
            yield return new WaitForFixedUpdate();
        }
    }

	void StopVelocity()
	{
		currentRigid.velocity = Vector3.zero;
		currentRigid.angularVelocity = Vector3.zero;
	}
}
