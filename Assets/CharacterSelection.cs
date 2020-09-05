using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public struct PlayerInfo
{
    public enum PlayerElement { Fire, Ice, Lightning, Leaf};
    public PlayerElement element;
    public string devicePath;
};

public class CharacterSelection : MonoBehaviour
{
    [Header("InputActions for joining setup")]
    [SerializeField] private InputAction joinAction = null;
    [SerializeField] private InputAction joinSplit1Action = null;
    [SerializeField] private InputAction joinSplit2Action = null;
    [SerializeField] private const string SplitControlSchemeName1 = "SplitKeyboard_1";
    [SerializeField] private const string SplitControlSchemeName2 = "SplitKeyboard_2";

    [Header("Sprites")]
    [SerializeField] private Sprite controller_sprite = null;
    [SerializeField] private Sprite keyboard_sprite = null;
    [SerializeField] private Sprite splitkeyboard_1_sprite = null;
    [SerializeField] private Sprite splitkeyboard_2_sprite = null;
    [SerializeField] private Sprite no_player_sprite = null;

    [Header("Selection UI")]
    [SerializeField] private CharacterSelectionUI[] players_selection_UI = new CharacterSelectionUI[4];

    [Header("Exit Event")]
    [SerializeField] private UnityEvent OnPlayerExitCharacterSelectionEvent = null;

    [Header("Start Event")]
    [SerializeField] private UnityEvent OnStartGameEvent = null;

    protected List<PlayerInfo> _playerInfo;
    protected bool[] _availableElements;
    protected PlayerInputManager _playerInputManager;
    private bool keyboard_1_joined = false;
    private bool keyboard_2_joined = false;

    [Header("Audio")]
    [SerializeField] AudioSource JoinSound = null;
    [SerializeField] AudioSource LeaveSound = null;

    private void Awake()
    {
        _availableElements = new bool[4] { true, true, true, true};
        _playerInfo = new List<PlayerInfo>();
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.currentControlScheme.Equals(SplitControlSchemeName1))
        {
            // If keyboard 1 not joined, set as joined, else switch control scheme to split_2
            if (!keyboard_1_joined)
            {
                keyboard_1_joined = true;
            }
            else
            {
                playerInput.SwitchCurrentControlScheme(SplitControlSchemeName2, playerInput.devices.ToArray());
                keyboard_2_joined = true;
            }
        }
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
                    case SplitControlSchemeName1:
                        players_selection_UI[i].SetupJoinedPlayer(splitkeyboard_1_sprite);
                        break;
                    case SplitControlSchemeName2:
                        players_selection_UI[i].SetupJoinedPlayer(splitkeyboard_2_sprite);
                        break;
                    default:
                        players_selection_UI[i].SetupJoinedPlayer(keyboard_sprite);
                        break;
                }
                playerInput.name = "Player_" + i;
                playerInput.actions["Exit"].performed += OnPlayerExit;      //segure para sair
                playerInput.actions["Start"].performed += StartGame;     //aperte start para começar
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
        playerInput.actions["Start"].performed -= StartGame;
        playerInput.actions["Exit"].performed -= OnPlayerExit;                          //não está funcionando quando nenhum jogador foi adicionado já que depende do PlayerInput

        // Set player_keyboard_1 as false if he is leaving
        if (playerInput.currentControlScheme.Equals(SplitControlSchemeName1))
        {
            keyboard_1_joined = false;
        }
        else if (playerInput.currentControlScheme.Equals(SplitControlSchemeName2))
        {
            keyboard_2_joined = false;
        }
        Debug.Log($"Left player{(PlayerInfo.PlayerElement) i}");
        LeaveSound?.Play();
    }

    private void OnPlayerExit(InputAction.CallbackContext obj) => OnPlayerExitCharacterSelectionEvent?.Invoke();

    private void StartGame(InputAction.CallbackContext obj) => OnStartGameEvent?.Invoke();

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
        _playerInputManager.EnableJoining();
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
