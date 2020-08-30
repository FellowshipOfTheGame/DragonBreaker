using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HidePlayButton : MonoBehaviour
{
    [SerializeField] private Button _PlayButton = null;
    [SerializeField] private Image _playButtonImg = null;
    [SerializeField] private int minimumToDisplay = 2;
    private AudioSource readyDrum;

    private PlayerInputManager _manager = null;

    private void Awake()
    {
        _manager = GetComponent<PlayerInputManager>();
        readyDrum = gameObject.GetComponent<AudioSource>();
    }

    public void DisplayCheck()
    {
        if(_manager.playerCount < minimumToDisplay)
        {
            _playButtonImg.enabled = false;
        }
        else
        {
            _playButtonImg.enabled = true;
            //_PlayButton.Select(); --> faz quando o player entra com o controle ja ir direto pro jogo (se tiver 2 jogadores)
            readyDrum.Play();
            Debug.Log("Play Button enabled.");
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput) => DisplayCheck();
    public void OnPlayerLeft(PlayerInput playerInput) => DisplayCheck();
}
