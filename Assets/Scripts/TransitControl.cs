using UnityEngine;

public class TransitControl : MonoBehaviour
{
    public Spawner spawnedObjects;

    private GameObject currentCandy;

    void Update()
    {
        currentCandy = spawnedObjects.GetCurrentObject();
        if (currentCandy != null) {
            if (currentCandy.transform.localScale.x < 0.2f)
			{
                transform.position = currentCandy.transform.position;
			}
        }
    }
}
