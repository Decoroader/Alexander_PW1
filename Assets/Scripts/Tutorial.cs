//#define flagIlliyaAwesomeVertion

using System.Collections;
using UnityEngine;


public class Tutorial : MonoBehaviour
{
    public CommonDataSettings commonData;
    public GameController gameController;
    public Spawner spawner;
    public Light pointCandyLight;
    public Light spotReceiverLight;

    public GameObject[] tutorialCandyPrefabs;
    public GameObject tutorialReceiver;

    public GameObject unclicked;
    public GameObject clicked;
    public GameObject leftRightArrow;
    public GameObject rightLeftArrow;

    private float tutorialTime = 0.5f;
    private bool isPressedMouseButton;
    private bool isTutorialActive;
    private Vector3 clickPosition = new Vector3(+0.3f, 0.33f, -0.7f);

#if !flagIlliyaAwesomeVertion
    private Rigidbody currRigid;
    private float speedToReceiver = 4.5f;
   
    private Vector3 candyPosition;
    //private Vector3 clickPosition0 = new Vector3(-4.75f, 0.3f, -0.57f);
    //private Vector3 clickPosition;
    private Vector3 scaledReceiver = new Vector3(1.8f, 1.8f, 1.2f);
    private Vector3 initReceiverScale;
    private Coroutine easyTutorial = null;
#endif

    void Awake()
    {
        candyPosition = commonData.easyCandyPosition;
        initReceiverScale = tutorialReceiver.transform.localScale;
        if (commonData.currentDifficulty != commonData.difficulty)
        {
            isTutorialActive = true;
            gameController.isGameActive = false;

            commonData.difficulty = commonData.currentDifficulty;
            if (commonData.currentDifficulty <= 2)
                easyTutorial = StartCoroutine(EasyTutorial());
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
            tutorialReceiver.transform.localScale = initReceiverScale;
            pointCandyLight.enabled = false;
            spotReceiverLight.enabled = false;
            if(easyTutorial != null)
                StopCoroutine(easyTutorial);

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

    // ------> scale version <--------
	IEnumerator EasyTutorial()
	{
        Vector3 initCandyScale;
        float scaleCoeff = 1.2f;

        while (isTutorialActive)
		{
			GameObject currentTutCandy =
				Instantiate(tutorialCandyPrefabs[1], candyPosition, tutorialCandyPrefabs[1].transform.rotation);
			currRigid = currentTutCandy.GetComponent<Rigidbody>();
            initCandyScale = currentTutCandy.transform.localScale;
            yield return new WaitForSeconds(tutorialTime);

            pointCandyLight.enabled = true;
            currentTutCandy.transform.localScale = 
                new Vector3(initCandyScale.x * scaleCoeff, initCandyScale.y * scaleCoeff, initCandyScale.z * scaleCoeff);
			yield return new WaitForSeconds(tutorialTime);

            pointCandyLight.enabled = false;
            spotReceiverLight.enabled = true;
            currentTutCandy.transform.localScale = initCandyScale;
            tutorialReceiver.transform.localScale = scaledReceiver;
            yield return new WaitForSeconds(tutorialTime);

            unclicked.SetActive(true);
            unclicked.transform.position = clickPosition;
            yield return new WaitForSeconds(tutorialTime);

            clicked.SetActive(true);
            unclicked.SetActive(false);
            clicked.transform.position = clickPosition;
			currentTutCandy.transform.position = new Vector3(clickPosition.x - 0.25f, candyPosition.y, candyPosition.z);
			currRigid.AddForce(Vector3.forward * speedToReceiver, ForceMode.Impulse); // is run
																					  // move the candy to the receiverside
			yield return new WaitForSeconds(tutorialTime / 2);

            unclicked.SetActive(true);
            clicked.SetActive(false);
            tutorialReceiver.transform.localScale = initReceiverScale;
            yield return new WaitForSeconds(tutorialTime / 2);

            unclicked.SetActive(false);
            yield return new WaitForSeconds(tutorialTime / 2);
         
            spotReceiverLight.enabled = false;
        }
    }
	// ------> scale version <--------

#endif
    IEnumerator MidPlusTutorial()
    {
        Vector3 startTap_Click = new Vector3(-2.3f, 1.0f, -3.9f);
        Instantiate(tutorialCandyPrefabs[1], candyPosition, tutorialCandyPrefabs[1].transform.rotation);
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
