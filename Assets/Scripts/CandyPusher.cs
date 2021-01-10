using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public CandyShifter candyShifter;

    private Rigidbody currentRigid;
    private float speedDinamicObject = 3;
    private float hightBound = 17;
    private int speedGame;
    private float tubeDistance = 8.8f;

    private Vector3 startFlyToHeadPos;
    private Vector3 startTransitToHeadPos;
    
    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        speedGame = 199;
        startFlyToHeadPos = new Vector3(0, 1, tubeDistance);
        startTransitToHeadPos = new Vector3(0, 1, -1.3f);
    }

    void Update()
    {
		if (Input.GetMouseButtonUp(0))                      // the cundy runs in the head side
			currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse); 

        if (candyShifter.GetMissedCounter() > speedGame)    // for the case the player click down and holds for too long time
            currentRigid.useGravity = true;         // this period will shorten as the level rises

        if (transform.position.z > hightBound)      // destroy the candy behind the head
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
            Vector3 initScale = transform.localScale;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            StartCoroutine(TubeTransit(initScale));
        }
    }
    IEnumerator TubeTransit(Vector3 outScale)       
	{
        int ttt = 0;
        Vector3 startMoveInTubePos = startTransitToHeadPos;

        while (transform.position.z < tubeDistance)
        {
            transform.position = new Vector3(startMoveInTubePos.x, startMoveInTubePos.y,
                startMoveInTubePos.z + (float)(ttt++) / 10);
            yield return new WaitForFixedUpdate();
        }
        transform.position = startFlyToHeadPos;       // 
        transform.localScale = outScale;
        currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse);
    }
}
