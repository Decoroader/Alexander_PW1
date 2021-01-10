using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsCandy;
    
    private GameObject currentCandy;

    private Vector3 prefabPosition;
    private float rangeX = 7;
    private float rangeMaxZ = -2.5f;
    private float rangeMinZ = -4.5f;

    void Start()
    {
        //InvokeRepeating("SpawnCandy", 1.0f, 3f);
        SpawnCandy();
    }
    
    private void SpawnCandy()
	{
        int prefabIndex = Random.Range(0, prefabsCandy.Length);
        prefabPosition = new Vector3(Random.Range(-rangeX, rangeX), 1, Random.Range(rangeMinZ, rangeMaxZ));
        currentCandy = Instantiate(prefabsCandy[prefabIndex], prefabPosition, Quaternion.identity) as GameObject;
	}

    public GameObject GetCurrentObject()
	{
        return currentCandy;
	}
}
