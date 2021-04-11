using UnityEngine;

public class MusicMenu : MonoBehaviour
{
    public CommonDataSettings commonData;

    public GameObject backgroundPlayerPrefab;
    
    void Start()
    {
        Instantiate(backgroundPlayerPrefab);
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
