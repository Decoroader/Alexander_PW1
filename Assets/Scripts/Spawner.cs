using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameController gameController;
    public ParticleSystem spawnCandyEffect;
    public CommonDataSettings commonSettings;

    public GameObject[] prefabsCandy;
    
    private GameObject currentCandy;
    private Vector3 prefabPosition;
    private float rangeX = 7f;
    private float rangeMaxZ = -1.5f;
    private float rangeMinZ = -4.5f;
    private float spawDelay = 0.21f;
    private bool midDifficulty;

    private int prefabIndex; 

    void Start()
    {
        StartCoroutine(NextObjectSpawner());
        if (commonSettings.currentDifficulty >= 2)
            midDifficulty = true;
        else
            midDifficulty = false;
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
                // !!! string between this comment allow make game more fast & difficult
                if ((((prefabPosition.x != currentCandy.transform.position.x) || 
                    (prefabPosition.z != currentCandy.transform.position.z)) && midDifficulty) ||
                // !!! string between this comment allow make game more fast & difficult (instead string below)

                    ((prefabPosition.z != currentCandy.transform.position.z) && !midDifficulty))
                
                // when candy moved from it's instantiate position wait minFlyTime and spawn new candy
                {
                    yield return new WaitForSeconds(spawDelay);
                    SpawnCandy();
                }
            }
            else
                SpawnCandy();
        }
        // for exclude (by gravity fall) the last candy from fly to a receiver ->
        currentCandy.GetComponent<Rigidbody>().useGravity = true;   
    }
}
