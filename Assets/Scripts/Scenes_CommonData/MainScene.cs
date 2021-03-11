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
        StartCoroutine(StartGame());
    }

	private void Update()
	{
        if (commonData.toMenu)
		{
            commonData.toMenu = false;
            StartCoroutine(SwitchScene(commonData.GAME, commonData.MENU));
        }
        if (commonData.toGame)
		{
            commonData.toGame = false;
            StartCoroutine(SwitchScene(commonData.MENU, commonData.GAME));
            //if (SceneManager.GetSceneByBuildIndex(commonData.SORRY) != null)
            //    showTimerSorryScene = 0;
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
        if (SceneManager.GetActiveScene().name == "MyGame")
            yield return SceneManager.UnloadSceneAsync(commonData.GAME);
        yield return SceneManager.LoadSceneAsync(commonData.GAME, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.GAME));
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