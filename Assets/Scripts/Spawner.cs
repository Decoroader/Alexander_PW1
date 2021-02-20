using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameController gameController;
    public ParticleSystem spawnCandyEffect;

    public GameObject[] prefabsCandy;
    
    private GameObject currentCandy;
    private Vector3 prefabPosition;
    private float rangeX = 7f;
    private float rangeMaxZ = -1.5f;
    private float rangeMinZ = -4.5f;
    private float minFlyTime = 0.5f;

    private int prefabIndex; 

    void Start()
    {
        StartCoroutine(NextObjectSpawner());
    }
	
	private void SpawnCandy()
	{
        prefabIndex = Random.Range(0, prefabsCandy.Length);
        prefabPosition = new Vector3(Random.Range(-rangeX, rangeX), 1, Random.Range(rangeMinZ, rangeMaxZ));
        currentCandy = Instantiate(prefabsCandy[prefabIndex], prefabPosition, prefabsCandy[prefabIndex].transform.rotation) as GameObject;
        spawnCandyEffect.transform.position = currentCandy.transform.position;
        spawnCandyEffect.Play();
    }

    IEnumerator NextObjectSpawner()
	{
        yield return new WaitForSeconds(1);         // delay before first spawn
        while (gameController.isGameActive)
        {
            yield return null;
            if (currentCandy != null)
            {
                if (prefabPosition.y != currentCandy.transform.position.y || prefabPosition.z != currentCandy.transform.position.z)
                // when candy moved from it's instantiate position wait minFlyTime and spawn new candy
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
