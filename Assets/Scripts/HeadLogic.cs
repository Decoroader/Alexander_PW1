using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLogic : MonoBehaviour
{
    [SerializeField]private Color[] A_HeadColors = new Color[] {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1),
        new Color(1, 0.92f, 0.016f),
        new Color(1, 0.89f, 0.005f),
    };

    public float coeffR = 0.75f;
    public float coeffG = 0.75f;
    public float coeffB = 0.75f;
    public float coeffY = 0.75f;
    public float coeffO = 0.75f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)){
            GetComponent<Renderer>().material.color = A_HeadColors[0];
        }
        if (Input.GetKeyUp(KeyCode.G)){
            GetComponent<Renderer>().material.color = A_HeadColors[1];
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            GetComponent<Renderer>().material.color = A_HeadColors[2];
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            GetComponent<Renderer>().material.color = A_HeadColors[3];
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            GetComponent<Renderer>().material.color = A_HeadColors[4];
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            GetComponent<Renderer>().material.color = A_HeadColors[0] * coeffR + A_HeadColors[1] * coeffG + 
                A_HeadColors[2]*coeffB + A_HeadColors[3] * coeffY + A_HeadColors[4] * coeffO;
        }
    }
}
