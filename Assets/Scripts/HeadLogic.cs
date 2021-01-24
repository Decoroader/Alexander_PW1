using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLogic : MonoBehaviour
{
    public GameController gameController;

    private Color[] A_HeadColors = new Color[5] {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1),
        new Color(1, 0.92f, 0.016f),
        new Color(1, 0.89f, 0.005f),
    };

    [SerializeField] private float coeffY = 0.21f;
    [SerializeField] private float coeffO = 0.5f;

    [SerializeField] private List<Color> headColorContainer = new List<Color> { };

    public GameObject nose;

    private Color trueColor;
    private int score_level = 1;
    private float tresholdColor = 3.9f;
    private int dearthTime = 5;

    void Start()
    {
        A_HeadColors[3] *= coeffY;
        A_HeadColors[4] *= coeffO;
		for (int iA = 0; iA < A_HeadColors.Length; iA++)
            headColorContainer.Add(A_HeadColors[iA]);

        trueColor = Get_HeadColor();
        GetComponent<Renderer>().material.color = trueColor;
        nose.GetComponent<Renderer>().material.color = trueColor;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            Color tempC = A_HeadColors[0] + A_HeadColors[1] +
                A_HeadColors[2] + A_HeadColors[3] * coeffY + A_HeadColors[4] * coeffO;
            GetComponent<Renderer>().material.color = tempC;
        }   
        if (Input.GetKeyUp(KeyCode.Q))
            Application.Quit();
    }
    private void OnCollisionEnter(Collision candy)
	{
		if (candy.gameObject.CompareTag("DinamicObject"))
		{
			headColorContainer.Add(
                A_HeadColors[candy.gameObject.GetComponent<CandyPusher>().GetCurrentObjectIndex()]);  // added current color to the 
			headColorContainer.RemoveAt(0);                                 // removed 1st element for save List lenght

            Color tempColor = Get_HeadColor();
            GetComponent<Renderer>().material.color = tempColor;
            nose.GetComponent<Renderer>().material.color = tempColor;

            if (tempColor == trueColor)
            {
                gameController.UpdateLevel_Score(++score_level);
            }
            if (tempColor.r > tresholdColor || tempColor.g > tresholdColor || tempColor.b > tresholdColor) { 
                gameController.GameOver();
                Debug.Log("call overColor sound...");
            }
            StopAllCoroutines();
            StartCoroutine(FeedTimer());
        }
    }
	
    private Color Get_HeadColor()                 // sum of the all colors in the List<Color>, and coloring head
	{
        Color headColor = Color.black;

        foreach (Color iColor in headColorContainer)
            headColor += iColor;
        return headColor;
    }

    IEnumerator FeedTimer()
    {
        int feedTimer = 1;
        while (gameController.isGameActive)
        {
            yield return new WaitForSeconds(5);
            if (++feedTimer > dearthTime)
            {
                gameController.GameOver();
                Debug.Log("call beep, as diyng...");
            }
            else
                Debug.Log("yum-yum sound...");
        }
    }
}
