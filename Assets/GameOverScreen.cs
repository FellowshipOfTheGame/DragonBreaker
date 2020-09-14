using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen = null;
    [SerializeField] private Text _text = null;
    public Button firstButtonToSelect;

    // Start is called before the first frame update
    void Start()
    {
        MultiplayerManager.Instance.onGameOver += OnGameOver;
    }

    private void OnGameOver(int winner_id)
    {

        if (_gameOverScreen == null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            _gameOverScreen.SetActive(true);
        }
        PlayerInfo.PlayerElement element = (PlayerInfo.PlayerElement)winner_id;
        _text.text = $"GUERREIRO DE {element.ToString()} VENCEU!";
        MultiplayerManager.Instance.DeactivatePlayersInputs();
    }

    private void OnDestroy()
    {
        if(MultiplayerManager.Instance != null)
            MultiplayerManager.Instance.onGameOver -= OnGameOver;
    }
}