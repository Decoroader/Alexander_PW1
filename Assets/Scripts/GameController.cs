using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// UI in the game and timers control 
/// mb should it called UIConrol?
/// </summary>
public class GameController : MonoBehaviour
{
    public CommonDataSettings commonData;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI hungryTimeText;
    public Image candyTime;
    public Button playBtn;
    public Button optionsBtn;
    public Light verticalLight;
    public Light horizontalLight;
    public Light pointLight;
    public AudioClip clickSound;
    public int gameSpeed = 230;
    public bool isGameActive;
    public bool gameOver;
    public bool hungry;
    public int hungryTimer;
    public GameObject backgroundPlayerPrefab;

    private int speedDiscrette = 30;
    private int maxSpeed = 50;
    private int timer = 111;
    private int hungryTreshold = 15;
    private int fadeControl = 90;

    //private Color gameOverLight = new Color(0.1f, 0, 0);
    private AudioSource playerAudio;
    private int level = 0;
    private int score = 0;
    private Coroutine candyTimer;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        StartCoroutine(CommonTimer());
        UpdateSpeedLevel();

        hungry = false;
        hungryTimeText.text = "Hungry " + 0;
        hungryTimeText.color = Color.green;
        StartCoroutine(HungryTimer());

        StartCoroutine(AlphaFade());
        
        candyTime.fillAmount = 0;

        gameOver = false;

        Instantiate(backgroundPlayerPrefab);
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
        profitText.color = Color.yellow;
        profitText.text = "+" + level;
        fadeControl = 0;
    }

    IEnumerator AlphaFade() // provide lerp fading the number of the hit candy profit  
	{
		while (true)
		{
            if (profitText.color.a > 0)
                profitText.color = new Color(profitText.color.r, profitText.color.g, profitText.color.b,
                    Mathf.Cos(fadeControl++ * Mathf.PI / 180) * 1.5f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CommonTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (isGameActive)
            {
                timeText.text = "Time " + timer--;
                if (timer < 0)
                    GameOver();
            }
        }
    }
    IEnumerator HungryTimer()
    {
        while (true)
        {
            if (isGameActive && hungry)
            {
                if(hungryTimer >= (hungryTreshold / 3) && hungryTimer < (hungryTreshold * 2 / 3))
                    hungryTimeText.color = Color.yellow;
                else if(hungryTimer >= (hungryTreshold * 2 / 3))
                    hungryTimeText.color = Color.red;
                else
                    hungryTimeText.color = Color.green;
             
                hungryTimeText.text = "Hungry " + ++hungryTimer;
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
        gameOver = true;
        StopAllCoroutines();
        playBtn.gameObject.SetActive(true);
        verticalLight.enabled = false;
        horizontalLight.enabled = false;
        pointLight.enabled = false;
    }
    public void PlayGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.reload = true;
    }
    public void OptionsGame()
    {
        playerAudio.PlayOneShot(clickSound, 1.0f);
        commonData.toMenu = true;
    }
}
