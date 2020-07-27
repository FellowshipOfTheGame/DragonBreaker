using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen = null;
    [SerializeField] private Text _text = null;

    // Start is called before the first frame update
    void Start()
    {
        MultiplayerManager.Instance.onGameOver += OnGameOver;
    }

    private void OnGameOver(int winner_id)
    {
        if(_gameOverScreen == null)
        {
            transform.GetChild(0).gameObject.SetActive(true);  
        }
        else
        {
            _text.text = $"JOGADOR {winner_id} VENCEU!";
            _gameOverScreen.SetActive(true);
        }
    }
}
