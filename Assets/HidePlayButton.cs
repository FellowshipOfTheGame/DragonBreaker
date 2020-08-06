using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HidePlayButton : MonoBehaviour
{
    [SerializeField] private Button _PlayButton;
    [SerializeField] private Image _playButtonImg;
    [SerializeField] private int minimumToDisplay = 2;

    private PlayerInputManager _manager;

    private void Awake()
    {
        _manager = GetComponent<PlayerInputManager>();
    }

    public void DisplayCheck()
    {
        Debug.Log(_manager.playerCount);
        if(_manager.playerCount < minimumToDisplay)
        {
            _playButtonImg.enabled = false;
            Debug.Log("Play Button disabled.");
        }
        else
        {
            _playButtonImg.enabled = true;
            //_PlayButton.Select(); --> faz quando o player entra com o controle ja ir direto pro jogo (se tiver 2 jogadores)
            Debug.Log("Play Button enabled.");
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput) => DisplayCheck();
    public void OnPlayerLeft(PlayerInput playerInput) => DisplayCheck();
}
