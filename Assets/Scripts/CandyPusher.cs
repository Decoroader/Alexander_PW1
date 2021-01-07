using UnityEngine;

public class CandyPusher : MonoBehaviour
{
    public CandyShifter candyShifter;

    private Rigidbody currentRigid;
    private float speedDinamicObject = 3;
    private float hightBound = 33;
    private int speedGame;

    private Vector3 startFlyPos = new Vector3(0, 1, 11);
    
    void Start()
    {
        currentRigid = GetComponent<Rigidbody>();
        speedGame = 99;
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
            transform.position = startFlyPos;
            currentRigid.AddForce(Vector3.forward * speedDinamicObject, ForceMode.Impulse);
        }

    }
}
