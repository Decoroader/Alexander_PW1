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

    private Vector3 mousePos;
    private float mousePosZ;

    private void Start()
	{
        camMain = Camera.main;
        currentRigid = GetComponent<Rigidbody>();
        waitingTime = GameObject.Find("GameController").GetComponent<GameController>().gameSpeed;
        mousePosZ = -17.0f;

        StartCoroutine(CandyPushControl());
    }
    private float RandomTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }
    private void PushCandy()
	{
        currentRigid.AddForce(Vector3.forward * speedOnOpenSpace, ForceMode.Impulse); // is run
                                                                                      // move the candy to the receiverside
        currentRigid.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
    }

//#if UNITY_STANDALONE || UNITY_WEBGL
    IEnumerator CandyPushControl()
    {
        float OnClickCursorCoordinateX = 0;

        bool isPressedMouseButton = false;

        Vector3 startCandyPosition = transform.position;

        while (true)
        {
            if (currentRigid.useGravity)                        // stop scan mouse for the candy shifting 
                break;
            if (startCandyPosition.z != transform.position.z)   // stop scan mouse, in the tutorial candy pushed
                break;
            
            mousePos = Input.mousePosition;
            mousePos.z = mousePosZ;
            mousePos = camMain.ScreenToWorldPoint(mousePos);// this is tranformation to the game coordinate
                                                            // it's obviosly, but one time I lost too many time w/o this info
            if (Input.GetMouseButtonDown(0) && !isPressedMouseButton) 
            {
                OnClickCursorCoordinateX = mousePos.x;      // get the X coordinate of the cursor on the first clicked frame
                isPressedMouseButton = true;
                if (commonData.difficulty == 3)
                    StartCoroutine(WaitForPlayerDecision());
            }
            if (isPressedMouseButton)
            {
                if (commonData.difficulty <= 2)
                {
                    transform.position = new Vector3(-OnClickCursorCoordinateX,
                            transform.position.y, transform.position.z);
                    isPressedMouseButton = false;
                }
                else
                {
                    float shiftX = OnClickCursorCoordinateX - mousePos.x;
                    OnClickCursorCoordinateX = mousePos.x;

                    transform.position = new Vector3(transform.position.x + shiftX,
                            transform.position.y, transform.position.z);
                }
            }
            if (Input.GetMouseButtonUp(0))              // runs the cundy in the receiver side?
            {
                PushCandy();
                break;
            }
            yield return null;
        }
        StopAllCoroutines();
    }
//#elif UNITY_IOS || UNITY_ANDROID
//    IEnumerator LeftRightSlide(){
//        Touch playerTouch = null;
//        while (true)
//        {
//            if (currentRigid.useGravity)                // stop scan mouse for the candy shifting 
//                break;
//            if(playerTouch == null){
//                if (Input.touchCount > 0) 
//                    playerTouch = Input.GetTouch(0);
//            }
//            if (playerTouch.phase == TouchPhase.Moved)  // mover of the dynamic object, like the cursor moves
//            { 
//                float shiftX = playerTouch.deltaPosition.x * coeffCoordinateMousToObject;
                
//                transform.position = new Vector3(transform.position.x - shiftX,
//                    transform.position.y, transform.position.z);
//            }
//            if (playerTouch.phase == TouchPhase.Ended)  // the cundy runs in the reciever side
//                break;
//            yield return new WaitForFixedUpdate();
//        }
//    }
//#endif
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
