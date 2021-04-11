using System.Collections;
using UnityEngine;

public class BackgroundPlayer : MonoBehaviour
{
    public CommonDataSettings commonData;

    public AudioSource[] A_background;

    private int currentIndex;
    private float maxVolume;

    void Start()
    {
        currentIndex = 0;
        maxVolume = A_background[0].volume;
    }

    void Update()
    {
        if(currentIndex != commonData.musicIndex)
		{
            A_background[currentIndex].Stop();
            currentIndex = commonData.musicIndex;
            StartCoroutine(LerpUpVolume());
        }
    }
    IEnumerator LerpUpVolume()
	{
        A_background[currentIndex].Play();
        A_background[currentIndex].volume = 0;
        int changeVolumeTimer = 50;
        float volumeIncreaser = (float)(maxVolume / 50);
        while (changeVolumeTimer-- > 0) {
            yield return new WaitForFixedUpdate();
            A_background[currentIndex].volume += volumeIncreaser;
        }
	}
}
