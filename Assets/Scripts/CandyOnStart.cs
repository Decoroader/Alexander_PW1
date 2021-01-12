﻿using System.Collections;
using UnityEngine;

public class CandyOnStart : MonoBehaviour
{
    private float coeffCoordinateMousToObject = 0.05f;
    private float initMouseCoordinate;

    private void Start()
	{
        StartCoroutine(LeftRightSlide());
    }

	IEnumerator LeftRightSlide()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                initMouseCoordinate = Input.mousePosition.x; // get the X coordinate of the cursor

            if (Input.GetMouseButton(0))    // mover of the dynamic object, like the cursor moves
            {
                float shiftX = (initMouseCoordinate - Input.mousePosition.x) * coeffCoordinateMousToObject;
                initMouseCoordinate = Input.mousePosition.x;
                transform.position = new Vector3(transform.position.x - shiftX,
                    transform.position.y, transform.position.z);
            }

			if (Input.GetMouseButtonUp(0))              // the cundy runs in the reciever side
				break;
			yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}