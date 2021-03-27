﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public CommonDataSettings commonData;
    public GameController gameController;
    public Spawner spawner;

    public GameObject unclicked;
    public GameObject clicked;
    public GameObject[] tutorialCandyPrefabs;

    private Rigidbody currRigid;
    private float speedToReceiver = 4.5f;
    private float tutorialTime = 0.5f;
    private bool isPressedMouseButton;
    private bool isTutorialActive;
    private Vector3 prefPosition0 = new Vector3(+2.5f, 1.0f, -3.0f);
    private Vector3 prefPosition1 = new Vector3(-2.5f, 1.0f, -3.0f);
    private Vector3 prefabPosition;
    private Vector3 clickPosition0 = new Vector3(-4.75f, 0.3f, -0.57f);
    private Vector3 clickPosition1 = new Vector3(+0.3f, 0.3f, -0.57f);
    private Vector3 clickPosition;

    private Vector3 redReseiverArrow = new Vector3(-5.9f, -1.7f, -3.0f);
    private Vector3 redCandyArrow = new Vector3(+3.39f, -1.79f, -5.9f);
    private Vector3 blueReseiverArrow = new Vector3(+1.0f, -1.5f, -3.0f);
    private Vector3 blueCandyArrow = new Vector3(-3.3f, -1.79f, -5.9f);

    void Awake()
    {
        if (commonData.currentDifficulty != commonData.difficulty)
        {
            isTutorialActive = true;
            gameController.isGameActive = false;

            commonData.difficulty = commonData.currentDifficulty;
            if (commonData.currentDifficulty == 1)
                StartCoroutine(EasyTutorial());
            else
                StartCoroutine(MidPlusTutorial());
        }
        else
        {
            gameController.isGameActive = true;
            isTutorialActive = false;
            GetComponent<Tutorial>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPressedMouseButton)
		{
            isPressedMouseButton = true;
        }
		if (isPressedMouseButton)
		{
            isTutorialActive = false;
            clicked.SetActive(false);
            unclicked.SetActive(false);

            StartCoroutine(Delay_StartGame());
        }
    }
    private int IndexGenerator()
	{
        return Random.Range(0, 2);
	}
    IEnumerator EasyTutorial()
    {
        while (isTutorialActive)
        {
            int prefIndex = IndexGenerator();
            if (prefIndex == 0)
            {
                prefabPosition = prefPosition0;
                clickPosition = clickPosition0;
            }
            else
            {
                prefabPosition = prefPosition1;
                clickPosition = clickPosition1;
            }
            GameObject currentTutCandy = 
                Instantiate(tutorialCandyPrefabs[prefIndex], prefabPosition, tutorialCandyPrefabs[prefIndex].transform.rotation);
            currRigid = currentTutCandy.GetComponent<Rigidbody>();
            yield return new WaitForSeconds(tutorialTime);

            unclicked.SetActive(true);
            unclicked.transform.position = clickPosition;
            yield return new WaitForSeconds(tutorialTime);

            clicked.SetActive(true);
            clicked.transform.position = clickPosition;
            currentTutCandy.transform.position = new Vector3(clickPosition.x, prefabPosition.y, prefabPosition.z);
            currRigid.AddForce(Vector3.forward * speedToReceiver, ForceMode.Impulse); // is run
                                                                                      // move the candy to the receiverside
            yield return new WaitForSeconds(tutorialTime / 2);

            clicked.SetActive(false);
            yield return new WaitForSeconds(tutorialTime);

            unclicked.SetActive(false);
        }

    }
    IEnumerator MidPlusTutorial()
    {
        unclicked.SetActive(true);
        yield return new WaitForSeconds(tutorialTime);

        clicked.SetActive(true);
        unclicked.SetActive(false);
        clicked.transform.position = new Vector3(0.3f, 0, -0.51f);
        int timeCounter = 17;
        while (timeCounter-- > 0)
        {
            yield return new WaitForFixedUpdate();
            clicked.transform.position = new Vector3(clicked.transform.position.x - 0.15f, 0, -0.51f); ;
        }

        unclicked.SetActive(true);
        unclicked.transform.position = clicked.transform.position;
        clicked.SetActive(false);
        yield return new WaitForSeconds(tutorialTime);

        unclicked.SetActive(false);
    }
    IEnumerator Delay_StartGame() 
    {
        yield return new WaitForSeconds(1);
        gameController.isGameActive = true;
        GetComponent<Tutorial>().enabled = false;
    }
}
