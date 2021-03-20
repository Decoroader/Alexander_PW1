using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CommonDataSettings commonData;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI hungryTimeText;
    public Image candyTime;
    public Button restartBtn;
    public Button quitBtn;
    public Light verticalLight;
    public Light horizontalLight;
    public Light pointLight;
    public AudioClip clickSound;
    public int gameSpeed = 230;
    public bool isGameActive;
    public bool hungry;
    public int hungryTimer;

    public GameObject unclicked;
    public GameObject clicked;

    private int speedDiscrette = 30;
    private int maxSpeed = 50;
    private int timer = 111;
    private float tutorialTime = 0.5f;
    private int hungryTreshold = 15;

    //private Color gameOverLight = new Color(0.1f, 0, 0);
    private AudioSource playerAudio;
    private int level = 0;
    private int score = -1;
    private Coroutine candyTimer;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        StartCoroutine(CommonTimer());
        UpdateSpeedLevel();
        UpdateScore();

        if (commonData.currentDifficulty != commonData.difficulty)
        {
            commonData.difficulty = commonData.currentDifficulty;
            unclicked.SetActive(true);
            unclicked.transform.position = new Vector3(0.3f, 0, -0.5f);
            if (commonData.currentDifficulty == 1)
                StartCoroutine(EasyTutorial());
            else
                StartCoroutine(MidPlusTutorial());
        }

        hungry = false;
        hungryTimeText.text = "Hungry " + 0;
        hungryTimeText.color = Color.green;
        StartCoroutine(HungryTimer());
        candyTime.fillAmount = 0;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
            Application.Quit();
        if (commonData.startCandyTime)
        {
            commonData.startCandyTime = false;

            candyTime.fillAmount = 1;
            if (candyTimer == null)
                candyTimer = StartCoroutine(CandyTimer());
        }
    }
    public void UpdateSpeedLevel()
    {
        levelText.text = "Speed Level: " + ++level;

        if (gameSpeed != maxSpeed)
        {
            if (gameSpeed - speedDiscrette > maxSpeed)
                gameSpeed -= speedDiscrette;
            else
                gameSpeed = maxSpeed;
        }
    }
    public void UpdateScore()
    {
        score += level;
        scoreText.text = "Score: " + score;
    }

    IEnumerator CommonTimer()
    {
        while (isGameActive)
        {
            timeText.text = "Time " + timer--;
            yield return new WaitForSeconds(1);
            if (timer < 0)
                GameOver();
        }
    }
    IEnumerator HungryTimer()
    {
        while (isGameActive)
        {
            if (hungry)
            {
                hungryTimeText.text = "Hungry " + ++hungryTimer;
                
                if(hungryTimer >= (hungryTreshold / 3) && hungryTimer < (hungryTreshold * 2 / 3))
                    hungryTimeText.color = Color.yellow;
                else if(hungryTimer >= (hungryTreshold * 2 / 3))
                    hungryTimeText.color = Color.red;
                else
                    hungryTimeText.color = Color.green;

                if (hungryTimer < 0)
                    GameOver();
            }
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator CandyTimer()
	{
		while (candyTime.fillAmount > 0)
		{
            candyTime.fillAmount -= (float)(1 / (float)gameSpeed);

            yield return new WaitForFixedUpdate();
        }
        candyTimer = null;
    }


    public void GameOver()
    {
        isGameActive = false;
        restartBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
        verticalLight.enabled = false;
        horizontalLight.enabled = false;
        pointLight.enabled = false;
    }
    public void RestartGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.reload = true;
    }
    public void QuitGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        Application.Quit();
    }
    IEnumerator EasyTutorial()
    {
        yield return new WaitForSeconds(tutorialTime);

        clicked.SetActive(true);
        clicked.transform.position = new Vector3(0.3f, 0, -0.51f);
        yield return new WaitForSeconds(tutorialTime/2);
        
        clicked.SetActive(false);
        yield return new WaitForSeconds(tutorialTime);

        unclicked.SetActive(false);
        
        isGameActive = true;
    }
    IEnumerator MidPlusTutorial()
    {
        yield return new WaitForSeconds(tutorialTime);

        clicked.SetActive(true);
        unclicked.SetActive(false);
        clicked.transform.position = new Vector3(0.3f, 0, -0.51f);
        int timeCounter = 17;
        while (timeCounter-- > 0)
        {
            yield return new WaitForFixedUpdate();
            clicked.transform.position = new Vector3(clicked.transform.position.x - 0.15f, 0, -0.51f); ;
        }

        unclicked.SetActive(true);
        unclicked.transform.position = clicked.transform.position;
        clicked.SetActive(false);
        yield return new WaitForSeconds(tutorialTime);

        unclicked.SetActive(false);

        isGameActive = true;
    }
}
