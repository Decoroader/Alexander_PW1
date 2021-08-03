using UnityEngine;

public class MusicMenu : MonoBehaviour
{
    public CommonDataSettings commonData;

    public GameObject backgroundPlayerPrefab;

    private Vector3 backgroundPosition = new Vector3(0, 3, 0);

    private void Awake()
    {
        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BGSound");
        backgroundMusic[0].transform.position = backgroundPosition;
    }

    public void Apply()
    {
        commonData.fromMusic_toGame = true;
    }
    
    public void ChooseMusicIndex(int indexMusic)
	{
        commonData.musicIndex = indexMusic;
    }
}
