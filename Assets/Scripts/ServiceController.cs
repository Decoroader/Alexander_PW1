using UnityEngine;

public class ServiceController : MonoBehaviour
{
    public CandyPusher candyPusherToController;
    public GameObject transitSphere3;



    void Start()
    {
        
    }

    void Update()
    {
		if (transitSphere3.activeInHierarchy)
		{
            MoveTransitObject();
		}
    }

    void MoveTransitObject()
	{

	}

    
}
