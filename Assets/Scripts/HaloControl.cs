﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloControl : MonoBehaviour
{
    public GameController gameController;
    public Animator comAnimator;
    public AudioClip overColorSound;
    public AudioClip yamSound;
    public AudioClip dyingSound;
    public AudioClip omnomRegularSound;
    public AudioClip omnomWowSound;


    private Color[] A_HeadColors = new Color[5] {
        new Color(1, 0, 0),
        new Color(0, 0.7f, 0),
        new Color(0, 0, 1),
        new Color(0.15f, 0.138f, 0.0025f),
        new Color(0.5f, 0.445f, 0.0025f),
    };

    [SerializeField] private List<Color> headColorContainer = new List<Color> { };

    private Color trueColor;
    private AudioSource playerAudio;

    private float tresholdColor = 3.9f;
    private int dearthTime = 3;
    private bool isCollisionAble = true;
    private readonly int timeOfCollisionLock = 15;

    //[SerializeField] private float coeffG = 0.7f;
    //[SerializeField] private float coeffY = 0.15f;
    //[SerializeField] private float coeffO = 0.5f;

    void Start()
    {
        //A_HeadColors[1] *= coeffG;
        //A_HeadColors[3] *= coeffY;
        //A_HeadColors[4] *= coeffO;
        
        playerAudio = GetComponent<AudioSource>();

        FillSaffleList();

        trueColor = Get_HeadColor();
        ColoringHead(trueColor);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Color tempC = A_HeadColors[0] + A_HeadColors[1] +
                A_HeadColors[2] + A_HeadColors[3] + A_HeadColors[4];
            ColoringHead(tempC);
            Debug.Log("Ideal color      " + tempC);
        }
    }
    private void OnCollisionEnter(Collision candy)
	{
        if (isCollisionAble)
        {
            if (candy.gameObject.CompareTag("DinamicObject") || candy.gameObject.CompareTag("ODinamicObject"))
            {
                headColorContainer.Add(
                    A_HeadColors[CandyPusher.GetCurrentObjectIndex(candy.gameObject)]);  // added current color to the 
                headColorContainer.RemoveAt(0);                                 // removed 1st element for save List lenght

                Color tempColor = Get_HeadColor();
                ColoringHead(tempColor);

                if (tempColor == trueColor)
                {
                    FillSaffleList();
                    gameController.UpdateLevel();
                    playerAudio.PlayOneShot(omnomWowSound, 1.0f);      // call om-nom wow sound
                }
                else if(tempColor.r > tresholdColor || tempColor.g > (tresholdColor*0.71f) || tempColor.b > tresholdColor)
                {
                    gameController.GameOver();
                    playerAudio.PlayOneShot(overColorSound, 1.0f);      // call overColor sound

                    comAnimator.SetTrigger("over_trg"); 
                }
                else
                    playerAudio.PlayOneShot(omnomRegularSound, 0.25f);  // call om-nom regular sound

                gameController.UpdateScore();

                StopAllCoroutines();
                StartCoroutine(FeedTimer());
                StartCoroutine(LockCollision());
            }
        }
    }
	
    private Color Get_HeadColor()                 // sum of the all colors in the List<Color>, and coloring head
	{
        Color headColor = Color.black;

        foreach (Color iColor in headColorContainer)
            headColor += iColor;
        return headColor;
    }

    private void ColoringHead(Color currColor)
	{
        GetComponent<Renderer>().material.color = currColor;
    }

    IEnumerator LockCollision()     // lock Collision since some candies collides more than one time
    {
        isCollisionAble = false;
        int timeOfLock = timeOfCollisionLock;
        while (timeOfLock-- > 0)
            yield return new WaitForFixedUpdate();
        if(gameController.isGameActive)
            isCollisionAble = true;
    }
    IEnumerator FeedTimer()         // time for life of the head without candy
    {
        int feedTimer = 1;
        while (gameController.isGameActive)
        {
            yield return new WaitForSeconds(5);
            if (++feedTimer > dearthTime)
            {
                gameController.GameOver();
                playerAudio.PlayOneShot(dyingSound, 1.0f);  // call dying sound

                comAnimator.SetTrigger("over_trg");
            }
            else
            {
                comAnimator.SetTrigger("yam_trg");
                playerAudio.PlayOneShot(yamSound, 1.0f);    // call yam sound
            }
        }
    }
    private void FillSaffleList() {
        headColorContainer.Clear();
        List<Color> tempColorList = new List<Color> { };
        for (int iA = 0; iA < A_HeadColors.Length; iA++)
            tempColorList.Add(A_HeadColors[iA]);
        while(tempColorList.Count > 0)
		{
            Color tempColor = tempColorList[Random.Range(0, tempColorList.Count)];
            headColorContainer.Add(tempColor);
            tempColorList.Remove(tempColor);
        }
    }
}