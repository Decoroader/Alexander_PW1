using System.Collections;
using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public CandyShifter candyShifter;

    private Rigidbody currentRigid;
    private float speedDinamicObject = 3;
    private float hightBound = 17;
    private float tubeDistance = 8.8f;

    private int speedGame;

    private Vector3 startFlyToHeadPos;
    private Vector3 startTransitToHeadPos;
    
    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        speedGame = 199;
        startFlyToHeadPos = new Vector3(0, 1, tubeDistance);
        startTransitToHeadPos = new Vector3(0, 1, -1.3f);
        StartCoroutine(WaitForDecision());
    }

    void Update()
    {
		if (Input.GetMouseButtonUp(0))                      // the cundy runs in the head side
			currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse); 

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
            transform.localScale *= 0.1f;           // reduce scale to hide the candy in the transit sphere
            StopAllCoroutines();
            StartCoroutine(TubeTransit());
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
    IEnumerator TubeTransit()       
	{
        currentRigid.useGravity = false;     // the candy falls and out from game

        int ttt = 0;
        Vector3 startMoveInTubePos = startTransitToHeadPos;

        while (transform.position.z < tubeDistance)
        {
            transform.position = new Vector3(startMoveInTubePos.x, startMoveInTubePos.y,
                startMoveInTubePos.z + (float)(ttt++) / 10);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Out from while");
        transform.position = startFlyToHeadPos;       // 
        transform.localScale *= 10;
        currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse);
    }
}
