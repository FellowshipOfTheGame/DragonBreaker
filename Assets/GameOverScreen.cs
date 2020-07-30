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
            /*string winner_name = "???";        <= podemos colocar os elementos dos guerreiros ao invés dos números dos players para identificar quem venceu

            switch (winner_id)
            {
                case 0:
                    winner_name = "DO FOGO";
                    break;

                case 1:
                    winner_name = "DO GELO";
                    break;

                case 2:
                    winner_name = "DO RAIO";
                    break;

                case 3:
                    winner_name = "DA FOLHA";
                    break;
            }*/
            _text.text = $"GUERREIRO {winner_id} VENCEU!";
            _gameOverScreen.SetActive(true);
        }
    }
}