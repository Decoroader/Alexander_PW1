using System.Collections;
using UnityEngine;

public class CandyOnStart : MonoBehaviour
{

    private float coeffCoordinateMousToObject = 0.05f;
    private float initMouseCoordinate = 0   ;

    private Rigidbody currentRigid;

    private void Start()
	{
        currentRigid = GetComponent<Rigidbody>();
        StartCoroutine(LeftRightSlide());
    }
#if UNITY_STANDALONE || UNITY_WEBGL
    IEnumerator LeftRightSlide()
    {
        while (true)
        {
            if (currentRigid.useGravity)            // stop scan mouse for the candy shifting 
                break;
            if (Input.GetMouseButtonDown(0)) 
                initMouseCoordinate = Input.mousePosition.x; // get the X coordinate of the cursor
            if (initMouseCoordinate > 0)
            { 
                if (Input.GetMouseButton(0))        // mover of the dynamic object, like the cursor moves
                {
                    float shiftX = (initMouseCoordinate - Input.mousePosition.x) * coeffCoordinateMousToObject;
                    initMouseCoordinate = Input.mousePosition.x;
                    transform.position = new Vector3(transform.position.x - shiftX,
                        transform.position.y, transform.position.z);
                }

                if (Input.GetMouseButtonUp(0))      // the cundy runs in the reciever side
                    break;
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
}
