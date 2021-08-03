﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public CommonDataSettings commonData;
    public GameObject offButton;
    public GameObject onButton;
    public GameObject hum50Hz;
    public GameObject computerSound;
    public GameObject backgroundPlayerPrefab;

    private float outXmin = 1.99f;
    private float outXmax = 2.01f;
    private float outZmin = 29.69f;
    private float outZmax = 29.71f;
    private bool isOutStartScene = false;
    
    private float humThresholdZ = -18.9f;
    private bool isArriveAtHum = false;

    private float compThresholdY = 4.1f;
    private bool isArriveAtComp = false;

    private float handXmin = 57f;
    private bool isRedButtonPressed = false;

    private GameObject child0Hand;
    private Vector3 handRotation;
    private Vector3 backgroundPosition = new Vector3(1.5f, 0.0f, 45.0f);

    void Start()
    {
        child0Hand = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (((transform.position.x >= outXmin) && (transform.position.x <= outXmax)) && ((transform.position.z >= outZmin) && (transform.position.z <= outZmax)) && !isOutStartScene)
        {
            commonData.fromStart_toGame = true;
            isOutStartScene = true;
        }

        if (transform.position.z > humThresholdZ && !isArriveAtHum)
        {
            TriggerAndObjectSwitchOn(ref isArriveAtHum, hum50Hz);
        }

        if (transform.position.y > compThresholdY && !isArriveAtComp)
        {
            TriggerAndObjectSwitchOn(ref isArriveAtComp, computerSound);
        }

        handRotation = child0Hand.transform.localEulerAngles;

        
        if ((handRotation.x > handXmin) && !isRedButtonPressed)
        {
            offButton.SetActive(false);
            TriggerAndObjectSwitchOn(ref isRedButtonPressed, onButton);
            DontDestroyOnLoad(Instantiate(backgroundPlayerPrefab, backgroundPosition, Quaternion.identity));
        }

    }
    private void TriggerAndObjectSwitchOn(ref bool catchFlag, GameObject soundObject)
    {
        catchFlag = true;
        soundObject.SetActive(true);
    }
}
