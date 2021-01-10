using UnityEngine;

public class CandyShifter : MonoBehaviour
{
    public float coeffCoordinateMousToObject = 0.05f;

    private float initMouseCoordinate;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            initMouseCoordinate = Input.mousePosition.x; // get the X coordinate of the cursor
        
        if (Input.GetMouseButton(0))  // mover of the dynamic object, like the cursor moves
		{
            float shiftX = (initMouseCoordinate - Input.mousePosition.x) * coeffCoordinateMousToObject;
            initMouseCoordinate = Input.mousePosition.x;
            transform.position = new Vector3(transform.position.x - shiftX,
                transform.position.y, transform.position.z);
        }
    }
}
