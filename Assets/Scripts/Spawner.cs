using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsCandy;
    
    public ParticleSystem spawnCandyEffect;

    private GameObject currentCandy;
    private Vector3 prefabPosition;
    private float rangeX = 7f;
    private float rangeMaxZ = -1.5f;
    private float rangeMinZ = -4.5f;
    private float gameSpeed = 2;
    private float minFlyTime = 0.5f;

    private int counter = 1; // temporary

    void Start()
    {
        StartCoroutine(NextObjectSpawner());
    }
	
	private void SpawnCandy()
	{
        int prefabIndex = Random.Range(0, prefabsCandy.Length);
        prefabPosition = new Vector3(Random.Range(-rangeX, rangeX), 1, Random.Range(rangeMinZ, rangeMaxZ));
        currentCandy = Instantiate(prefabsCandy[prefabIndex], prefabPosition, Quaternion.identity) as GameObject;
        if(counter % 10 == 0)
		{
            counter++;
            gameSpeed -= 0.5f; // temporary, should be changed to 1/coeff as -> to zero but never zero
        }
    }

    public GameObject GetCurrentObject()
	{
        return currentCandy;
	}

    IEnumerator NextObjectSpawner()
	{
        yield return new WaitForSeconds(1);         // delay before first spawn
        while (true)
        {
            yield return null;
            if (currentCandy != null)
            {
                if (prefabPosition.y != currentCandy.transform.position.y || prefabPosition.z != currentCandy.transform.position.z)
                {
                    yield return new WaitForSeconds(minFlyTime);
                    SpawnCandy();
                }
            }
            else
                SpawnCandy();
        }
    }
}
