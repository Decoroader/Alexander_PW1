using System.Collections;
using UnityEngine;

public class CandyOnStart : MonoBehaviour
{
    public CommonDataSettings commonData;

    private Camera camMain;
    private Rigidbody currentRigid;

    private float   speedOnOpenSpace = 5.5f;
    private float   torqueRange = 11;
    private int     waitingTime;

    private void Start()
	{
        camMain = Camera.main;
        currentRigid = GetComponent<Rigidbody>();
        waitingTime = GameObject.Find("GameController").GetComponent<GameController>().gameSpeed;

        StartCoroutine(LeftRightSlide());
    }
    private float RandomTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }

#if UNITY_STANDALONE || UNITY_WEBGL
    IEnumerator LeftRightSlide()
    {
        float initCursorCoordinateX = 0;

        bool isPressedMouseButton = false;          // I know about drys

        while (true)
        {
            if (currentRigid.useGravity)            // stop scan mouse for the candy shifting 
                break;
            if (Input.GetMouseButtonDown(0)) 
            { 
                initCursorCoordinateX = camMain.ScreenToWorldPoint(Input.mousePosition).x; // get the X coordinate of the cursor
                isPressedMouseButton = true;
                StartCoroutine(WaitForPlayerDecision());
            }
            if (isPressedMouseButton)
            {
                Vector3 cursor = camMain.ScreenToWorldPoint(Input.mousePosition);
                if (commonData.difficulty == 1)
                    transform.position = new Vector3(cursor.x,
                            transform.position.y, transform.position.z);
				else
				{
                    float shiftX = initCursorCoordinateX - cursor.x;
                    initCursorCoordinateX = cursor.x;
                    transform.position = new Vector3(transform.position.x - shiftX,
                            transform.position.y, transform.position.z);
                }
                if (Input.GetMouseButtonUp(0))              // runs the cundy in the receiver side?
                {
                    currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); // is run
                    // move the candy to the receiverside
                    currentRigid.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
                    // random rotation in the all axes
                    StopAllCoroutines();
                    break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
#elif UNITY_IOS || UNITY_ANDROID
    IEnumerator LeftRightSlide(){
        Touch playerTouch = null;
        while (true)
        {
            if (currentRigid.useGravity)                // stop scan mouse for the candy shifting 
                break;
            if(playerTouch == null){
                if (Input.touchCount > 0) 
                    playerTouch = Input.GetTouch(0);
            }
            if (playerTouch.phase == TouchPhase.Moved)  // mover of the dynamic object, like the cursor moves
            { 
                float shiftX = playerTouch.deltaPosition.x * coeffCoordinateMousToObject;
                
                transform.position = new Vector3(transform.position.x - shiftX,
                    transform.position.y, transform.position.z);
            }
            if (playerTouch.phase == TouchPhase.Ended)  // the cundy runs in the reciever side
                break;
            yield return new WaitForFixedUpdate();
        }
    }
#endif
    IEnumerator WaitForPlayerDecision()
	{
        commonData.startCandyTime = true;
		while (waitingTime-- > 0)
		{
            yield return new WaitForFixedUpdate();
        }
        currentRigid.useGravity = true;
    }
}
