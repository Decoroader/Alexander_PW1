using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public CommonDataSettings commonData;

    private int showTimerSorryScene = 75;

    private void Awake()
	{
        commonData.difficulty = 0;
        commonData.currentDifficulty = 1;
    }
        
    private void Start()
    {
        StartCoroutine(StartScene());
    }

	private void Update()
	{
        if (commonData.fromStart_toGame)
        {
            commonData.fromStart_toGame = false;
            StartCoroutine(SwitchScene(commonData.STARTSCENE, commonData.GAME));
        }

        if (commonData.toMenu)
		{
            commonData.toMenu = false;
            StartCoroutine(SwitchScene(commonData.GAME, commonData.MENU));
        }
        
        if (commonData.toMusic)
		{
            commonData.toMusic = false;
            StartCoroutine(SwitchScene(commonData.MENU, commonData.MUSIC));
        }

        if (commonData.fromMenu_toGame)
		{
            commonData.fromMenu_toGame = false;
            if(commonData.currentDifficulty == commonData.difficulty)
                commonData.difficulty += 1;

            StartCoroutine(SwitchScene(commonData.MENU, commonData.GAME));
        }

        if (commonData.fromMusic_toGame)
        {
            commonData.fromMusic_toGame = false;
            if (commonData.currentDifficulty == commonData.difficulty)
                commonData.difficulty += 1;
            StartCoroutine(SwitchScene(commonData.MUSIC, commonData.GAME));
        }

        if (commonData.sorry)
            StartCoroutine(SorryScene());
        if (commonData.reload)
            StartCoroutine(StartGame());
    }

    IEnumerator SwitchScene(int sceneForUnloadIndex, int sceneForLoadIndex)
    {
        if (SceneManager.GetSceneByBuildIndex(sceneForUnloadIndex) != null)
            yield return SceneManager.UnloadSceneAsync(sceneForUnloadIndex);

        yield return SceneManager.LoadSceneAsync(sceneForLoadIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneForLoadIndex));
    }
    IEnumerator StartGame()
    {
        commonData.reload = false;
        // if (SceneManager.GetActiveScene().buildIndex == 1) // this line for optimization as avoid strings in a code
        if (SceneManager.GetActiveScene().name == "MyGame")
            yield return SceneManager.UnloadSceneAsync(commonData.GAME);
        yield return SceneManager.LoadSceneAsync(commonData.GAME, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.GAME));
    }

    IEnumerator StartScene()
    {
        yield return SceneManager.LoadSceneAsync(commonData.STARTSCENE, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.STARTSCENE));
    }

    IEnumerator SorryScene()
    {
        commonData.sorry = false;
        yield return SceneManager.LoadSceneAsync(commonData.SORRY, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.SORRY));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(commonData.MENU));

        while (showTimerSorryScene-- > 0)
		{
            yield return new WaitForFixedUpdate();
        }
        yield return SceneManager.LoadSceneAsync(commonData.MENU, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.MENU));
        SceneManager.UnloadSceneAsync(commonData.SORRY);
        showTimerSorryScene = 75;
    }
}   