using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloControl : MonoBehaviour
{
    public GameController gameController;
    public Animator comAnimator;
    public AudioClip overColorSound;
    public AudioClip yamSound;
    public AudioClip dyingSound;
    public AudioClip omnomRegularSound;
    public AudioClip omnomWowSound;


    private Color[] A_HeadColors = new Color[5] {
        new Color(0.91f, 0, 0),
        new Color(0, 0.75f, 0),
        new Color(0, 0, 1.0f),
        new Color(0.91f, 0, 0),
        new Color(0, 0.75f, 0)
        //new Color(0.15f, 0.138f, 0.0025f),
        //new Color(0.5f, 0.445f, 0.0025f),
    };

    [SerializeField] private List<Color> headColorContainer = new List<Color> { };

    private Color trueColor;
    private AudioSource playerAudio;

    private float thresholdColorR = 3.65f;
    private float thresholdColorG = 3.01f;
    private float thresholdColorB = 4.01f;
    private int dearthTime = 3;
    private bool isCollisionAble = true;
    private readonly int timeOfCollisionLock = 15;

    private Coroutine lockCollisionCoroutine;

    //[SerializeField] private float coeffG = 0.7f;
    //[SerializeField] private float coeffY = 0.15f;
    //[SerializeField] private float coeffO = 0.5f;

    void Start()
    {
        //A_HeadColors[1] *= coeffG;
        //A_HeadColors[3] *= coeffY;
        //A_HeadColors[4] *= coeffO;
        
        playerAudio = GetComponent<AudioSource>();

        FillSaffleList();

        trueColor = Get_HeadColor();
        ColoringHead(trueColor);
    }

    // !!!!!!!!!!!!! don't delete !!!!!!!!!!!!!!!!!!!!!!!!!! this update for ideal color test
    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        Color tempC = A_HeadColors[0] + A_HeadColors[1] +
    //            A_HeadColors[2] + A_HeadColors[3] + A_HeadColors[4];
    //        ColoringHead(tempC);
    //        Debug.Log("Ideal color      " + tempC);
    //    }
    //}
    private void OnCollisionEnter(Collision candy)
	{
        if (isCollisionAble & gameController.isGameActive)
        {
            if (candy.gameObject.CompareTag("DinamicObject") || candy.gameObject.CompareTag("ODinamicObject"))
            {
                int rgbIndex = ObjectColorIndex.GetCurrentObjectIndex(candy.gameObject);
                AudioClip forPlayClip;
                if (rgbIndex <= 2)                  // check r, g, b colors only
                {
                    headColorContainer.Add(
                        A_HeadColors[rgbIndex]);    // current color added the last element for save List lenght
                    headColorContainer.RemoveAt(0); // for save List lenght 1st element removed
                

                    Color tempColor = Get_HeadColor();  
                    ColoringHead(tempColor);

                    if (tempColor == trueColor)
                    {
                        FillSaffleList();
                        gameController.UpdateSpeedLevel();
                        forPlayClip = omnomWowSound;                    // call om-nom wow sound
                    }
                    else if (tempColor.r > (thresholdColorR) || tempColor.g > (thresholdColorG) || tempColor.b > thresholdColorB)
                    {
                        gameController.GameOver();
                        forPlayClip = overColorSound;                   // call overColor sound

                        comAnimator.SetTrigger("over_trg");
                    }
                    else
                        forPlayClip = omnomRegularSound;                // call om-nom regular sound
                }
                else
                    forPlayClip = omnomRegularSound;                // call om-nom regular sound
                playerAudio.PlayOneShot(forPlayClip, 0.25f);        // play accordly voice

                gameController.UpdateScore();

                StopAllCoroutines();
                StartCoroutine(FeedTimer());
                lockCollisionCoroutine = StartCoroutine(LockCollision());
            }
        }
    }
	
    private Color Get_HeadColor()                 // sum of the all colors in the List<Color>, and coloring head
	{
        Color headColor = Color.black;

        foreach (Color iColor in headColorContainer)
            headColor += iColor;
        return headColor;
    }

    private void ColoringHead(Color currColor)
	{
        GetComponent<Renderer>().material.color = currColor;
    }

    IEnumerator LockCollision()     // lock Collision since some candies collides more than one time
    {
        isCollisionAble = false;
        int timeOfLock = timeOfCollisionLock;
        while (timeOfLock-- > 0)
            yield return new WaitForFixedUpdate();
        isCollisionAble = true;
    }
    IEnumerator FeedTimer()         // time for life of the head without candy
    {
        gameController.hungry = true;
        gameController.hungryTimer = 0;

        int feedTimer = 1;
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (gameController.isGameActive)
            {
                if (++feedTimer > dearthTime)
                {
                    gameController.GameOver();
                    playerAudio.PlayOneShot(dyingSound, 1.0f);  // call dying sound

                    comAnimator.SetTrigger("over_trg");
                    StopCoroutine(lockCollisionCoroutine);
                    isCollisionAble = false;
                }
                else
                {
                    comAnimator.SetTrigger("yam_trg");
                    playerAudio.PlayOneShot(yamSound, 1.0f);    // call yam sound
                }
            }
            else
                break;
        }
    }
    private void FillSaffleList() {
        headColorContainer.Clear();
        List<Color> tempColorList = new List<Color> { };
        for (int iA = 0; iA < A_HeadColors.Length; iA++)
            tempColorList.Add(A_HeadColors[iA]);
        while(tempColorList.Count > 0)
		{
            Color tempColor = tempColorList[Random.Range(0, tempColorList.Count)];
            headColorContainer.Add(tempColor);
            tempColorList.Remove(tempColor);
        }
    }
}
