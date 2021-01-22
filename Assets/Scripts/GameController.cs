using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public int gameSpeed = 230;
    public bool isGameActive = true;

    //[SerializeField]private int speedDiscrette = 30;
    private int speedDiscrette = 30;
    private int maxSpeed = 50;
    private int timer = 10;

    void Start()
    {
        UpdateLevel_Score(1);
        StartCoroutine(CommonTimer());
    }

    void Update()
    {
        
    }

    public void UpdateLevel_Score(int localScore)
	{
        scoreText.text = "Score: " + localScore;

        if (gameSpeed != maxSpeed)
        {
            if (gameSpeed - speedDiscrette > maxSpeed)
                gameSpeed -= speedDiscrette;
            else
                gameSpeed = maxSpeed;
        }
        Debug.Log("gameSpeed = " + gameSpeed);
    }
    IEnumerator CommonTimer()
	{
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("timer = " + timer);
            if (timer <= 0)
                GameOver();
            timeText.text = "Time " + timer--;
        }
    }
    private void GameOver()
	{
        isGameActive = false;

    }
}
