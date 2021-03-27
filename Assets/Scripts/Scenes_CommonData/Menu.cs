using UnityEngine;

public class Menu : MonoBehaviour
{
    public AudioClip clickSound;

    public CommonDataSettings commonData;

    private AudioSource playerAudio;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void FromMenuToGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.toGame = true;
    }
    public void LowDifficulty()
	{
        playerAudio.PlayOneShot(clickSound, 1.0f);

        commonData.currentDifficulty = 1;
    }
    public void MidDifficulty()
	{
        playerAudio.PlayOneShot(clickSound, 1.0f);

        commonData.currentDifficulty = 2;
    }
    public void HardDifficulty()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);

        commonData.currentDifficulty = 3;
        commonData.sorry = true;
    }
    public void BackGround()
    {
        commonData.sorry = true;
    }
    public void Voice()
    {
        commonData.sorry = true;
    }
}
