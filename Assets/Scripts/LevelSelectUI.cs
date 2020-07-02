using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    public Dropdown NPlayers = null;

    public void StartLevel(string sceneName)
    {
        MultiplayerManager.NumberOfPlayers = NPlayers.value + 1;
        SceneManager.LoadScene(sceneName);
    }

}
