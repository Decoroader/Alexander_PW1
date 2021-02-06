using System.Collections;
using UnityEngine;

public class CandyDissolution : MonoBehaviour
{
    public ParticleSystem smokePS;
    public ParticleSystem currentSmokePS;

    private bool isDisableAllCollisions = false;

    private Vector3 tempLocalScale;
    private int timeScaleFall = 75;
    private float speedScaleFall = 0.001333333f;

    private Rigidbody currentRigid;
    private Quaternion initSmokeRotation;

    void Start()
    {
        currentSmokePS = Instantiate(smokePS, transform.position, smokePS.transform.rotation);
        initSmokeRotation = currentSmokePS.transform.rotation;
        currentSmokePS.transform.parent = gameObject.transform;
        currentRigid = GetComponent<Rigidbody>();
        tempLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision toHead_toGround)
    {
        if (!isDisableAllCollisions)
        {
            if (toHead_toGround.gameObject.CompareTag("Head"))           // destroy the candy in the head
                Destroy(gameObject, 0.19f);
            if (toHead_toGround.gameObject.CompareTag("DinamicObject"))  // collision with other candy
                currentRigid.useGravity = true;                 // the other candy falls and out from the game
            if (toHead_toGround.gameObject.CompareTag("Ground"))
            {
                isDisableAllCollisions = true;
                StartCoroutine(FallingToDestroy());
            }
        }
    }
    IEnumerator FallingToDestroy()
    {
        tempLocalScale = transform.localScale;
        while (transform.localScale.x > tempLocalScale.x * 0.25f)
        {
            yield return new WaitForSeconds(5);
            currentRigid.velocity = Vector3.zero;
            if(currentSmokePS.transform.parent != null)
                currentSmokePS.transform.parent = null;
            currentSmokePS.transform.rotation = initSmokeRotation;
            currentSmokePS.transform.position = new Vector3(transform.position.x, 
                transform.position.y - 0.25f, transform.position.z);
            currentSmokePS.transform.localScale *= 0.9f;

            currentSmokePS.Play();
            StartCoroutine(SoftScaleDown());
        }
        currentSmokePS.transform.parent = gameObject.transform;
        Destroy(gameObject, 0.3f);
    }

    IEnumerator SoftScaleDown()
    {
        int iteratorFallingScale = timeScaleFall;
        while (iteratorFallingScale-- > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - tempLocalScale.x * speedScaleFall,
                transform.localScale.y - tempLocalScale.y * speedScaleFall, transform.localScale.z - tempLocalScale.z * speedScaleFall);
            yield return new WaitForFixedUpdate();
        }
    }
}
