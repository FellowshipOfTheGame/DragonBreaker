﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public struct PlayerInfo
{
    public enum PlayerElement { Fire, Ice, Leaf, Lightning};
    public PlayerElement element;
    public string devicePath;
};

public class CharacterSelection : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite controller_sprite = null;
    [SerializeField] private Sprite keyboard_sprite = null;
    [SerializeField] private Sprite no_player_sprite = null;

    [Header("Selection UI")]
    [SerializeField] private CharacterSelectionUI[] players_selection_UI = new CharacterSelectionUI[4];

    [Header("Exit Event")]
    [SerializeField] private UnityEvent OnPlayerExitCharacterSelectionEvent = null;

    protected List<PlayerInfo> _playerInfo;
    protected bool[] _availableElements;

    [Header("Audio")]
    [SerializeField] AudioSource JoinSound;
    [SerializeField] AudioSource LeaveSound;

    private void Start()
    {
        _availableElements = new bool[4] { true, true, true, true};
        _playerInfo = new List<PlayerInfo>();
    }
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joining");
        for (int i = 0; i < _availableElements.Length; i++)
        {
            if (_availableElements[i])
            {
                _playerInfo.Add(new PlayerInfo { element = (PlayerInfo.PlayerElement)i, devicePath = playerInput.devices[0].layout });
                Debug.Log($"Joined player {_playerInfo[_playerInfo.Count - 1]}");
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
                playerInput.actions["Exit"].performed += OnPlayerExit;
                _availableElements[i] = false;
                break;
            }
        }
        JoinSound.Play();
    }
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        //Convert to int Player_{i}
        int i = playerInput.gameObject.name[7] - 48;
        _availableElements[i] = true;
        players_selection_UI[i].SetupLeftPlayer(no_player_sprite);
        _playerInfo.Remove(_playerInfo.Find((match) => i.Equals((int)match.element)));
        playerInput.actions["Exit"].performed -= OnPlayerExit;                          //não está funcionando quando nenhum jogador foi adicionado
        Debug.Log($"Left player{(PlayerInfo.PlayerElement) i}");
        LeaveSound.Play();
    }

    private void OnPlayerExit(InputAction.CallbackContext obj) => OnPlayerExitCharacterSelectionEvent?.Invoke();

    public void LoadLevel()
    {
        if (_playerInfo.Count < 2)
        {
            return;
        }
        SetPlayerPrefs();
        LoadSceneManager.LoadNextScene();
    }

    public void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("Number of Players", _playerInfo.Count);
        Debug.Log(_playerInfo.Count);
        for (int i = 0; i < _playerInfo.Count; i++)
        {
            PlayerPrefs.SetString($"Player_{i}_device", _playerInfo[i].devicePath);
            PlayerPrefs.SetInt($"Player_{i}_element", (int)_playerInfo[i].element);
            Debug.Log(_playerInfo[i].devicePath);
            Debug.Log((int)_playerInfo[i].element);
        }
    }

    private void OnEnable()
    {
        FindObjectOfType<PlayerInputManager>().EnableJoining();
    }

    void OnDisable()
    {
        var players = FindObjectsOfType<PlayerInput>();

        foreach (var p in players)
        {
            OnPlayerLeft(p);
            Destroy(p.gameObject);
        }
    }
}
