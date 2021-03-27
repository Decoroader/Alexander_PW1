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
    //private 

    private int prefabIndex; 

    void Start()
    {
        StartCoroutine(NextObjectSpawner());
        if (commonSettings.currentDifficulty >= 2)
            midDifficulty = true;
        else
            midDifficulty = false;
    }

    private Vector3 PrefabPosition()
	{
        if(currentCandy == null)
            return new Vector3(Random.Range(-rangeX, rangeX), 1, Random.Range(rangeMinZ, rangeMaxZ));

        int colorIndex = ObjectColorIndex.GetCurrentObjectIndex(currentCandy);
        float shiftNewCandyX = 1.51f;
        float tempXPosition = Random.Range(-rangeX, rangeX);

        switch (colorIndex)
        {
            case 0:
                if (tempXPosition >= -5.75f && tempXPosition <= -4.25f)
                    tempXPosition += shiftNewCandyX;
                    break;
            case 1:
                if (tempXPosition >= -3.25f && tempXPosition <= -1.75f)
                    tempXPosition += shiftNewCandyX;
                    break;
            case 2:
                if (tempXPosition >= -0.75f && tempXPosition <= 0.75f)
                    tempXPosition += shiftNewCandyX;
                    break;
            case 3:
                if (tempXPosition >= 1.75f && tempXPosition <= 3.25f)
                    tempXPosition -= shiftNewCandyX;
                    break;
            case 4:
                if (tempXPosition >= 4.25f && tempXPosition <= 5.75f)
                    tempXPosition -= shiftNewCandyX;
                    break;
		}
        return new Vector3(tempXPosition, 1, Random.Range(rangeMinZ, rangeMaxZ));
    }
    private void SpawnCandy()
	{
        prefabPosition = PrefabPosition();
        prefabIndex = Random.Range(0, prefabsCandy.Length);

        currentCandy = Instantiate(prefabsCandy[prefabIndex], prefabPosition, prefabsCandy[prefabIndex].transform.rotation) as GameObject;
        spawnCandyEffect.transform.position = currentCandy.transform.position;
        spawnCandyEffect.Play();
    }

    IEnumerator NextObjectSpawner()
	{
        yield return new WaitForSeconds(1);         // delay before first spawn

        while (true)
        {
            yield return null;
            if (gameController.isGameActive)
            {
                if (currentCandy != null)
                {
                    // when candy moved from it's instantiate position wait minFlyTime and spawn new candy

                    // !!! string between this comment allow make game more fast & difficult
                    if ((((prefabPosition.x != currentCandy.transform.position.x) ||
                        (prefabPosition.z != currentCandy.transform.position.z)) && midDifficulty) ||
                        // !!! string between this comment allow make game more fast & difficult (instead string below)
                        ((prefabPosition.z != currentCandy.transform.position.z) && !midDifficulty))
                    {
                        yield return new WaitForSeconds(spawDelay);
                        SpawnCandy();
                    }
                }
                else
                    SpawnCandy();
            }
            else if (gameController.gameOver)
                break;
        }
        // for exclude (by gravity fall) the last candy from fly to a receiver ->
        currentCandy.GetComponent<Rigidbody>().useGravity = true;   
    }
}
