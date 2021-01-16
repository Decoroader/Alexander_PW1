using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLogic : MonoBehaviour
{
    public Spawner spawnerData;

    [SerializeField]private Color[] A_HeadColors = new Color[5] {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1),
        new Color(1, 0.92f, 0.016f),
        new Color(1, 0.89f, 0.005f),
    };

    [SerializeField] private float coeffY = 0.21f;
    [SerializeField] private float coeffO = 0.5f;

    private List<Color> headColorContainer = new List<Color> {
        new Color(1, 0, 0),
        new Color(0, 1, 0),
        new Color(0, 0, 1)
    };

    void Start()
    {
        headColorContainer.Add(A_HeadColors[3] * coeffY);
        headColorContainer.Add(A_HeadColors[4] * coeffO);
        GetHeadColor();
    }

    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.R)){
        //    GetComponent<Renderer>().material.color = A_HeadColors[0];
        //}
        //if (Input.GetKeyUp(KeyCode.G)){
        //    GetComponent<Renderer>().material.color = A_HeadColors[1];
        //}
        //if (Input.GetKeyUp(KeyCode.B))
        //{
        //    GetComponent<Renderer>().material.color = A_HeadColors[2];
        //}
        //if (Input.GetKeyUp(KeyCode.Y))
        //{
        //    GetComponent<Renderer>().material.color = A_HeadColors[3];
        //}
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    GetComponent<Renderer>().material.color = A_HeadColors[4];
        //}
        if (Input.GetKeyUp(KeyCode.S))
        {
			GetComponent<Renderer>().material.color = A_HeadColors[0] + A_HeadColors[1] +
				A_HeadColors[2] + A_HeadColors[3] * coeffY + A_HeadColors[4] * coeffO;
		}
    }
	private void OnCollisionEnter(Collision candy)
	{
		if (candy.gameObject.CompareTag("DinamicObject"))
		{
			headColorContainer.Add(A_HeadColors[spawnerData.GetCurrentPrefabIndex()]);  // added current color to the 
			headColorContainer.RemoveAt(0);                                             // removed 1st element for save List lenght
		}
	}
	private void GetHeadColor()                 // sum of the all colors in the List<Color>, and coloring head
	{
        Color headColor = Color.black;

        foreach (Color iColor in headColorContainer)
            headColor += iColor;
        GetComponent<Renderer>().material.color = headColor;
    }
}
