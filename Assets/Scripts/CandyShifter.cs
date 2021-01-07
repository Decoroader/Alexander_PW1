using UnityEngine;

public class CandyShifter : MonoBehaviour
{
    public float coeffCoordinateMousToObject = 0.05f;
    private float initMouseCoordinate;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            initMouseCoordinate = Input.mousePosition.x;

        if (Input.GetMouseButton(0))
		{
            float shiftX = (initMouseCoordinate - Input.mousePosition.x) * coeffCoordinateMousToObject;
            initMouseCoordinate = Input.mousePosition.x;
            transform.position = new Vector3(transform.position.x - shiftX,
                transform.position.y, transform.position.z);
        }
        
    }
}
