//#define flagIlliyaAwesomeVertion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public CommonDataSettings commonData;
    public GameController gameController;
    public Spawner spawner;
    public Light pointCandyLight;
    public Light spotReceiverLight;

    public GameObject[] tutorialCandyPrefabs;
    public GameObject tutorialReceiver;
    public GameObject tutorialTubeReceiver;     // for detach(till tutorial works) from the Receiver to avoid tube scaling

    public GameObject unclicked;
    public GameObject clicked;
    public Button tutorialPlayButton;

    private GameObject currentTutCandy;
    private Vector3 initCandyScale;
    private float scaleCoeff = 1.3f;
    private Rigidbody currRigid;
    private float tutorialTime = 0.5f;
    private Vector3 clickPosition = new Vector3(+0.3f, 0.33f, -0.7f);

#if !flagIlliyaAwesomeVertion
    private float speedToReceiver = 4.5f;
   
    private Vector3 candyPosition;
    private Vector3 scaledReceiver = new Vector3(1.8f, 1.8f, 1.2f);
    private Vector3 initReceiverScale;
    private Coroutine currentTutorial = null;
#endif

    void Awake()
    {
        // TODO: change tutorial hand
        candyPosition = commonData.easyCandyPosition;
        initReceiverScale = tutorialReceiver.transform.localScale;

        if (commonData.currentDifficulty != commonData.difficulty)
        {
            gameController.isGameActive = false;

            commonData.difficulty = commonData.currentDifficulty;
            if (commonData.currentDifficulty <= 2)
                currentTutorial = StartCoroutine(EasyTutorial());
            else
                currentTutorial = StartCoroutine(HardTutorial());
            tutorialTubeReceiver.transform.parent = null;
            tutorialPlayButton.gameObject.SetActive(true);
        }
        else
        {
            tutorialPlayButton.gameObject.SetActive(false);
            gameController.isGameActive = true;
            GetComponent<Tutorial>().enabled = false;
        }
    }

	
    public void PlayChecker()
	{
        clicked.SetActive(false);
        unclicked.SetActive(false);
        tutorialReceiver.transform.localScale = initReceiverScale;
        tutorialTubeReceiver.transform.parent = tutorialReceiver.transform;
        pointCandyLight.enabled = false;
        spotReceiverLight.enabled = false;
        if (currentTutorial != null)
            StopCoroutine(currentTutorial);

        Destroy(currentTutCandy, 0.1f);

        StartCoroutine(Delay_StartGame());

        tutorialPlayButton.gameObject.SetActive(false);
    }

#if flagIlliyaAwesomeVertion
    // !!!!!!!!!!!!!!!Illiya version!!!!!!!!!!!!!!!!!!
    IEnumerator EasyTutorial()
    {
        Instantiate(tutorialCandyPrefabs[1], prefPosition1, tutorialCandyPrefabs[1].transform.rotation);
        yield return new WaitForSeconds(tutorialTime);

        unclicked.SetActive(true);
        unclicked.transform.position = clickPosition1;
        while (true)
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
    }
    // !!!!!!!!!!!!!!!Illiya version!!!!!!!!!!!!!!!!!!
#else

    private void GetCandyAndData(Vector3 candyPos)
	{
        currentTutCandy = 
            Instantiate(tutorialCandyPrefabs[1], candyPos, tutorialCandyPrefabs[1].transform.rotation);
        currRigid = currentTutCandy.GetComponent<Rigidbody>();
        initCandyScale = currentTutCandy.transform.localScale;
    }
    // ------> scale version <--------
    IEnumerator EasyTutorial()
	{
        while (true)
		{
            GetCandyAndData(candyPosition);
			
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
    IEnumerator HardTutorial()
    {
        Vector3 startHardPosition = new Vector3(-2.3f, 1.0f, -5.5f);
        Vector3 startCandyPositionHard = 
            new Vector3(startHardPosition.x - 0.2f, startHardPosition.y, startHardPosition.z + 1.5f );
        
        while (true)
        {
            GetCandyAndData(startCandyPositionHard);
            yield return new WaitForSeconds(tutorialTime);
            
            currentTutCandy.transform.localScale =
                new Vector3(initCandyScale.x * scaleCoeff, initCandyScale.y * scaleCoeff, initCandyScale.z * scaleCoeff);
            yield return new WaitForSeconds(tutorialTime);

            currentTutCandy.transform.localScale = initCandyScale;
            unclicked.transform.position = startHardPosition;
            clicked.transform.position = startHardPosition;
            unclicked.SetActive(true);
            yield return new WaitForSeconds(tutorialTime / 2);

            tutorialReceiver.transform.localScale = scaledReceiver;
            spotReceiverLight.enabled = true;
            clicked.SetActive(true);
            unclicked.SetActive(false);
            int timeCounter = 53;
            float shifter = 0.05f;
            while (timeCounter-- > 0)
            {
                yield return new WaitForFixedUpdate();
                clicked.transform.position = 
                    new Vector3(clicked.transform.position.x + shifter, startHardPosition.y, startHardPosition.z);
                currentTutCandy.transform.position = 
                    new Vector3(currentTutCandy.transform.position.x + shifter, startCandyPositionHard.y, startCandyPositionHard.z);
            }
            currRigid.AddForce(Vector3.forward * speedToReceiver, ForceMode.Impulse);
            tutorialReceiver.transform.localScale = initReceiverScale;
            spotReceiverLight.enabled = false;

            unclicked.SetActive(true);
            unclicked.transform.position = clicked.transform.position;
            clicked.SetActive(false);
            yield return new WaitForSeconds(tutorialTime / 2);
        }
    }
    IEnumerator Delay_StartGame() 
    {
        yield return new WaitForSeconds(1);
        gameController.isGameActive = true;
        GetComponent<Tutorial>().enabled = false;
    }
}
