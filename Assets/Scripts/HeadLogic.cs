using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLogic : MonoBehaviour
{
    [SerializeField]private Color[] A_HeadColors = new Color[5] {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1),
        new Color(1, 0.92f, 0.016f),
        new Color(1, 0.89f, 0.005f),
    };

    [SerializeField] private float coeffY = 0.21f;
    [SerializeField] private float coeffO = 0.5f;

    [SerializeField]private List<Color> headColorContainer = new List<Color> {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1)
    };

    void Start()
    {
        A_HeadColors[3] *= coeffY;
        A_HeadColors[4] *= coeffO;
        headColorContainer.Add(A_HeadColors[3]);
        headColorContainer.Add(A_HeadColors[4]);
        Get_Set_HeadColor();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
			GetComponent<Renderer>().material.color = A_HeadColors[0] + A_HeadColors[1] +
				A_HeadColors[2] + A_HeadColors[3] * coeffY + A_HeadColors[4] * coeffO;
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
			headColorContainer.RemoveAt(0);                                             // removed 1st element for save List lenght
            Get_Set_HeadColor();
		}
	}
	private void Get_Set_HeadColor()                 // sum of the all colors in the List<Color>, and coloring head
	{
        Color headColor = Color.black;

        foreach (Color iColor in headColorContainer)
            headColor += iColor;
        GetComponent<Renderer>().material.color = headColor;
    }
}
