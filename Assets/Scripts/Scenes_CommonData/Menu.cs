﻿using UnityEngine;

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

    private Vector3 backgroundPosition = new Vector3(0, 3, 0);

    private void Awake()
    {
        GameObject[] backgroundMusic = GameObject.FindGameObjectsWithTag("BGSound");
        backgroundMusic[0].transform.position = backgroundPosition;
    }
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();

        FrameSetter(commonData.currentDifficulty);
    }

    public void FromMenuToGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.fromMenu_toGame = true;
    }
    public void ChooseDifficulty(int chosenDifficulty)
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.currentDifficulty = chosenDifficulty;
        FrameSetter(commonData.currentDifficulty);
    }
   
    public void BackgroundMusic()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.toMusic = true;
    }
    public void Face()
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
