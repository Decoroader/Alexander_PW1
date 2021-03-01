using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool isOpenMouseInput = true;
    public CommonDataSettings commonData;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Button restartBtn;
    public Button quitBtn;
    public Light verticalLight;
    public AudioClip clickSound;
    public int gameSpeed = 230;
    public bool isGameActive = true;

    private int speedDiscrette = 30;
    private int maxSpeed = 50;
    private int timer = 111;
    private Color gameOverLight = new Color(0.1f, 0, 0);
    private AudioSource playerAudio;
    private int level = 0;
    private int score = -1;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        StartCoroutine(CommonTimer());
        UpdateLevel();
        UpdateScore();
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
            Application.Quit();
    }
    public void UpdateLevel()
	{
        levelText.text = "Level: " + ++level;

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
    
    public void GameOver()
	{
        isGameActive = false;
        restartBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
        verticalLight.color = gameOverLight;
        isOpenMouseInput = true;
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
}