using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public CommonDataSettings commonData;

    private int showTimerSorryScene = 75;

    void Start()
    {
        StartCoroutine(FirstStartScene());
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
            if (SceneManager.GetSceneByBuildIndex(commonData.SORRY) != null)
                showTimerSorryScene = 0;
        }
        if (commonData.sorry)
            StartCoroutine(SorryScene());
    }

    IEnumerator SwitchScene(int sceneForUnloadIndex, int sceneForLoadIndex)
    {
        if (SceneManager.GetSceneByBuildIndex(sceneForUnloadIndex) != null)
            yield return SceneManager.UnloadSceneAsync(sceneForUnloadIndex);

        yield return SceneManager.LoadSceneAsync(sceneForLoadIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneForLoadIndex));
    }
    IEnumerator FirstStartScene()
    {
        yield return SceneManager.LoadSceneAsync(commonData.GAME, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.GAME));
    }
    IEnumerator SorryScene()
    {
        commonData.sorry = false;
        yield return SceneManager.LoadSceneAsync(commonData.SORRY, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(commonData.SORRY));
		while (showTimerSorryScene-- > 0)
		{
            yield return new WaitForFixedUpdate();
        }
        SceneManager.UnloadSceneAsync(commonData.SORRY);
        showTimerSorryScene = 75;
    }
}   