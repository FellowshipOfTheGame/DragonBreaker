using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerInfo
{
    public enum PlayerElement { Fire, Ice, Lightning, Leaf};
    public PlayerElement element;
    public UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice> devices;
};

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Sprite controller_sprite = null;
    [SerializeField] private Sprite keyboard_sprite = null;
    [SerializeField] private Sprite no_player_sprite = null;
    [SerializeField] private CharacterSelectionUI[] players_selection_UI = new CharacterSelectionUI[4];

    protected List<PlayerInfo> _playerInfo;
    protected bool[] _availableElements;

    private void Start()
    {
        _availableElements = new bool[4] { true, true, true, true};
        _playerInfo = new List<PlayerInfo>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        for (int i = 0; i < _availableElements.Length; i++)
        {
            if (_availableElements[i])
            {
                _playerInfo.Add(new PlayerInfo { element = (PlayerInfo.PlayerElement)i, devices = playerInput.devices });
                switch (playerInput.currentControlScheme)
                {
                    case "Gamepad":
                        players_selection_UI[i].SetupJoinedPlayer(controller_sprite);
                        break;
                    case "Keyboard":
                        players_selection_UI[i].SetupJoinedPlayer(keyboard_sprite);
                        break;
                    default:
                        players_selection_UI[i].SetupJoinedPlayer(keyboard_sprite);
                        break;
                }
                playerInput.name = "Player_" + i;
                _availableElements[i] = false;
                break;
            }
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        //Convert to int Player_{i}
        int i = playerInput.gameObject.name[7] - 48;
        _availableElements[i] = true;
        players_selection_UI[i].SetupLeftPlayer(no_player_sprite);
    }
}
