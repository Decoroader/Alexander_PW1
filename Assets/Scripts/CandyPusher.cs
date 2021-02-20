using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public GameObject prefabTransit;
    public GameObject[] prefabsOpenCandy;
    
    private GameObject currentTransit;
    private float speedOnOpenSpace = 5.5f;
    private float speedInTube = 7;

    private float torqueRange = 11;

    private Rigidbody currentRigid;

    private Quaternion initRotation;
    private int countForMoveInTube = 0;
    private int countWaitToReceiver = 39;
    private int scaleTransformer = 3;
    private int waitingTime;
    private Vector3 tempLocalScale;

    private string receiverString = "Receiver";
    private int currentIndex;

    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        initRotation = transform.rotation;
        waitingTime = GameObject.Find("GameController").GetComponent<GameController>().gameSpeed;
        currentIndex = GetCurrentObjectIndex();
        StartCoroutine(WaitForPlayerDecision());
    }

    void Update()
    {
        if (countForMoveInTube-- > 0)       // move the candy in the tube
            transform.Translate(Vector3.forward * speedInTube * Time.deltaTime);
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
                    currentRigid.velocity *= -1;
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
                    gameObject.transform.position, prefabsOpenCandy[currentIndex].transform.rotation);
                gameObject.transform.position = Vector3.forward * (-11);
                Destroy(gameObject, 1);
            }
            else
            {
                transform.rotation = initRotation;
                transform.localScale = tempLocalScale;         // back the scale to show the candy in the track to the head
                currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse);
            }
            // move the candy to the head side

            countForMoveInTube = 0;
        }
    }
    private float RandomTorque()
	{
        return Random.Range(-torqueRange, torqueRange);
	}

    IEnumerator WaitForPlayerDecision()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        bool isPressedMouseButton = false;
        while (waitingTime-- > 0)                         // Wait For the Players Decision
        {
            if (Input.GetMouseButtonDown(0))            // 
                isPressedMouseButton = true;
            if (isPressedMouseButton)
            {
                if (Input.GetMouseButtonUp(0))              // runs the cundy in the receiver side?
                {
                    currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); // is run
                    // move the candy to the receiverside
                    currentRigid.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
                    // random rotation in the all axes
                    StartCoroutine(WaitForReciever());      // wait for hit the receiver
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        currentRigid.useGravity = true;         // the candy falls and out from the game
        yield return null;
#elif UNITY_IOS || UNITY_ANDROID
        while (waitingTime-- > 0)                         // Wait For the Players Decision
        {
            if (Input.GetMouseButtonDown(0))            // 
                isPressedMouseButton = true;
            if (isPressedMouseButton)
            {
                if (Input.GetMouseButtonUp(0))              // runs the cundy in the receiver side?
                {
                    currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); // is run
                    // move the candy to the receiverside
                    currentRigid.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
                    // random rotation in the all axes
                    StartCoroutine(WaitForReciever());      // wait for hit the receiver
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        currentRigid.useGravity = true;         // the candy falls and out from the game
        yield return null;
#endif
    }
    IEnumerator WaitForReciever()
	{
		while (true)
		{
			if (!(countWaitToReceiver-- > 0))           // while candy moving to the receiverside
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
    public static int GetCurrentObjectIndex(GameObject objectForIndex)
    {
        return (int)char.GetNumericValue(objectForIndex.name[objectForIndex.name.Length - 8]);
    }
    public int GetCurrentObjectIndex()
    {
        return (int)char.GetNumericValue(gameObject.name[gameObject.name.Length - 8]);
    }
}
