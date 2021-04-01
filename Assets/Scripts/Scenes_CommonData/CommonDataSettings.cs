using UnityEngine;

[CreateAssetMenu(menuName = "DataSettings")]
public class CommonDataSettings : ScriptableObject
{
    public bool enableTutorial;
    public int  difficulty;
    public int  currentDifficulty;

    public readonly int GAME = 1;
    public readonly int MENU = 2;
    public readonly int SORRY = 3;

    public bool toGame;
    public bool toMenu;
    public bool sorry;
    public bool reload;
    public bool startCandyTime;

    public Vector3 easyCandyPosition = new Vector3(+1.5f, 1.0f, -3.5f);


    private void Awake()
	{
        difficulty = 0;
        currentDifficulty = 1;

        toGame = false;
        toMenu = false;
        sorry = false;
        reload = false;
        startCandyTime = false;
    }
}
