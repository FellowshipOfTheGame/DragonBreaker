using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HidePlayButton : MonoBehaviour
{
    [SerializeField] private Image _PlayButton;
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
            _PlayButton.color = new Color (1, 1, 1, 0);
            //Play button is transparent when there are less than 2 players
        }
        else
        {
            _PlayButton.color = new Color(1, 1, 1, 1);
            //Play button is visible when there are two players or more
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput) => DisplayCheck();
    public void OnPlayerLeft(PlayerInput playerInput) => DisplayCheck();
}
