//#define flagIlliyaAwesomeVertion

using System.Collections;
using UnityEngine;


public class Tutorial : MonoBehaviour
{
    public CommonDataSettings commonData;
    public GameController gameController;
    public Spawner spawner;

    public GameObject[] tutorialCandyPrefabs;

    public GameObject unclicked;
    public GameObject clicked;
    public GameObject leftRightArrow;
    public GameObject rightLeftArrow;

    private float tutorialTime = 0.5f;
    private bool isPressedMouseButton;
    private bool isTutorialActive;
    private Vector3 prefPosition1 = new Vector3(-2.5f, 1.0f, -3.0f);
    private Vector3 clickPosition1 = new Vector3(+0.3f, 0.3f, -0.57f);

#if !flagIlliyaAwesomeVertion
    private Rigidbody currRigid;
    private float speedToReceiver = 4.5f;
    
    private Vector3 prefPosition0 = new Vector3(+2.5f, 1.0f, -3.0f);
   
    private Vector3 prefabPosition;
    private Vector3 clickPosition0 = new Vector3(-4.75f, 0.3f, -0.57f);
    private Vector3 clickPosition;

    private Vector3 redReseiverArrow0 = new Vector3(-5.97f, +2.3f, -0.3f);
    private Vector3 redCandyArrow0 = new Vector3(+3.39f, +1.89f, -3.0f);
    private Vector3 blueReseiverArrow1 = new Vector3(+1.0f, +2.3f, -0.3f);
    private Vector3 blueCandyArrow1 = new Vector3(-3.3f, +1.75f, -3.0f);
    private Vector3 reseiverArrowPosition;
    private Vector3 candyArrowPosition;
    private GameObject reseiverArrow;
    private GameObject candyArrow;
#endif

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
#if flagIlliyaAwesomeVertion
    // !!!!!!!!!!!!!!!Illiya version!!!!!!!!!!!!!!!!!!
    IEnumerator EasyTutorial()
    {
        Instantiate(tutorialCandyPrefabs[1], prefPosition1, tutorialCandyPrefabs[1].transform.rotation);
        yield return new WaitForSeconds(tutorialTime);

        unclicked.SetActive(true);
        unclicked.transform.position = clickPosition1;
        while (isTutorialActive)
        {
            yield return new WaitForSeconds(tutorialTime);

            clicked.SetActive(true);
            unclicked.SetActive(false);
            clicked.transform.position = clickPosition1;
            yield return new WaitForSeconds(tutorialTime / 2);

            clicked.SetActive(false);
            unclicked.SetActive(true);
            yield return new WaitForSeconds(tutorialTime);

        }
        unclicked.SetActive(false);
    }
    // !!!!!!!!!!!!!!!Illiya version!!!!!!!!!!!!!!!!!!
#else

    // ------> arrows version <--------
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
				candyArrowPosition = redCandyArrow0;
				candyArrow = rightLeftArrow;
				reseiverArrowPosition = redReseiverArrow0;
				reseiverArrow = leftRightArrow;
			}
			else
			{
				prefabPosition = prefPosition1;
				clickPosition = clickPosition1;
				candyArrowPosition = blueCandyArrow1;
				candyArrow = leftRightArrow;
				reseiverArrowPosition = blueReseiverArrow1;
				reseiverArrow = rightLeftArrow;
			}
			candyArrow.SetActive(false);
			reseiverArrow.SetActive(false);

			GameObject currentTutCandy =
				Instantiate(tutorialCandyPrefabs[prefIndex], prefabPosition, tutorialCandyPrefabs[prefIndex].transform.rotation);
			currRigid = currentTutCandy.GetComponent<Rigidbody>();
			StartCoroutine(ArrowControl());                 // blinking arrows
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
	// ------> arrows version <--------

	IEnumerator ArrowControl()
    {
        candyArrow.transform.position = candyArrowPosition;
        yield return new WaitForSeconds(tutorialTime/4);
        candyArrow.SetActive(true);
        yield return new WaitForSeconds(tutorialTime / 4);
        candyArrow.SetActive(false);
        yield return new WaitForSeconds(tutorialTime / 4);
        candyArrow.SetActive(true);
        yield return new WaitForSeconds(tutorialTime / 4);
        candyArrow.SetActive(false);

        reseiverArrow.transform.position = reseiverArrowPosition;
        reseiverArrow.SetActive(true);
        yield return new WaitForSeconds(tutorialTime / 4);
        reseiverArrow.SetActive(false);
        yield return new WaitForSeconds(tutorialTime / 4);
        reseiverArrow.SetActive(true);
        yield return new WaitForSeconds(tutorialTime / 4);
        reseiverArrow.SetActive(false);
    }
#endif
    IEnumerator MidPlusTutorial()
    {
        Vector3 startTap_Click = new Vector3(-2.3f, 1.0f, -3.9f);
        Instantiate(tutorialCandyPrefabs[1], prefPosition1, tutorialCandyPrefabs[1].transform.rotation);
        yield return new WaitForSeconds(tutorialTime);
        unclicked.SetActive(true);

        while (isTutorialActive)
        {
            unclicked.transform.position = startTap_Click;
            clicked.transform.position = startTap_Click;
            yield return new WaitForSeconds(tutorialTime / 2);

            clicked.SetActive(true);
            unclicked.SetActive(false);
            int timeCounter = 18;
            while (timeCounter-- > 0)
            {
                yield return new WaitForFixedUpdate();
                clicked.transform.position = new Vector3(clicked.transform.position.x + 0.15f, startTap_Click.y, startTap_Click.z); ;
            }

            unclicked.SetActive(true);
            unclicked.transform.position = clicked.transform.position;
            clicked.SetActive(false);
            yield return new WaitForSeconds(tutorialTime / 2);
        }
        unclicked.SetActive(false);
    }
    IEnumerator Delay_StartGame() 
    {
        yield return new WaitForSeconds(1);
        gameController.isGameActive = true;
        GetComponent<Tutorial>().enabled = false;
    }
}
