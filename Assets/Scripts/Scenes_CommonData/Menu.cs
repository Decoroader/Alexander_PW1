using System.Collections;
using UnityEngine.SceneManagement;
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

        commonData.difficulty_mid = false;
        commonData.difficulty_hight = false;
    }
    public void MidDifficulty()
	{
        playerAudio.PlayOneShot(clickSound, 1.0f);

        commonData.difficulty_mid = true;
    }
    public void HardDifficulty()
    {
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
