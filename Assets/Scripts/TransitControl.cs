using UnityEngine;

public class TransitControl : MonoBehaviour
{
    public Spawner spawnedObject;

    private GameObject currentCandy;

    void Update()
    {
        currentCandy = spawnedObject.GetCurrentObject();
        if ((currentCandy != null) && (currentCandy.transform.localScale.x < 0.5f))
            transform.position = new Vector3(currentCandy.transform.position.x, 
                currentCandy.transform.position.y, currentCandy.transform.position.z);
    }
}
