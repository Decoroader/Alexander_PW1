using UnityEngine;

public class Menu : MonoBehaviour
{
    public AudioClip clickSound;

    public CommonDataSettings commonData;
    
    public GameObject buttonEasy;
    public GameObject buttonMid;
    public GameObject buttonHard;
    public GameObject difficultyFrame;

    private AudioSource playerAudio;

    private Vector3 frameDifficultyPosition;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();

        FrameSetter(commonData.currentDifficulty);
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
        FrameSetter(commonData.currentDifficulty);
    }
    public void MidDifficulty()
	{
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.currentDifficulty = 2;
        FrameSetter(commonData.currentDifficulty);
    }
    public void HardDifficulty()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.currentDifficulty = 3;
        FrameSetter(commonData.currentDifficulty);
    }
    public void BackGround()
    {
        commonData.sorry = true;
    }
    public void Voice()
    {
        commonData.sorry = true;
    }
    private void FrameSetter(int difficulty)
	{
        switch (difficulty)
        {
            case 1:
                frameDifficultyPosition = buttonEasy.transform.position;
                break;
            case 2:
                frameDifficultyPosition = buttonMid.transform.position;
                break;
            case 3:
                frameDifficultyPosition = buttonHard.transform.position;
                break;
        }
        difficultyFrame.transform.position = frameDifficultyPosition;
    }
}
