using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public static void LoadSceneByBuildIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public void RestartSceneMet() => LoadSceneManager.RestartScene();

    public void LoadSceneByNameMet(string sceneName) => LoadSceneManager.LoadSceneByName(sceneName);

    public void LoadNextSceneMet() => LoadSceneManager.LoadNextScene();

    public void LoadSceneByBuildIndexMet(int buildIndex) => LoadSceneManager.LoadSceneByBuildIndex(buildIndex);
}
