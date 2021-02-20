using System.Collections;
using UnityEngine;

public class FaceControl : MonoBehaviour
{
    public Animator comAnimator;
    
    private bool lockTrigger = true;
    private readonly int timeOfTriggerLock = 19;

    void Start()
    {
        comAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider candy)
	{
        if (lockTrigger)
        {
            if (candy.gameObject.CompareTag("DinamicObject") || candy.gameObject.CompareTag("ODinamicObject"))
            {
                comAnimator.SetTrigger("om_nom_trg");
                StartCoroutine(LockTrigger());
            }

        }
    }
    IEnumerator LockTrigger()     // lock Trigger since some candies to triggers more than one time
    {
        lockTrigger = false;
        int timeOfLock = timeOfTriggerLock;
        while (timeOfLock-- > 0)
            yield return new WaitForFixedUpdate();

        lockTrigger = true;
    }
}
