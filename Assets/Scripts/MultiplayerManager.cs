using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    // Instance for singleton
    public static MultiplayerManager Instance { get; private set; }
    // Number of players to be instantiated at the start of the scene, need to be at least 1 and no more then available devices
    public static int NumberOfPlayers { get; set; } = 0;
    
    public GameObject[] PlayerPrefabs = { };
    public event Action<int> onGameOver;

    [SerializeField] private PlayerUI[] _playerUIs = { };
    [SerializeField] private int _playerCount = 0;
    [SerializeField] private PauseScreen _pauseScreen = null;

    private Dictionary<ReadOnlyArray<InputDevice>, int> _indexUI;
    private List<PlayerInput> _players = null;
    private Countdown _countdown = null;

    //Make it Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _playerCount = 0;
        _players = new List<PlayerInput>();
        _indexUI = new Dictionary<ReadOnlyArray<InputDevice>, int>();
        
        //InstantiatePlayers();
        InstantiatePlayersWithSetDevices();
    }

    public void InstantiatePlayers()
    {
        PlayerInputManager manager = GetComponent<PlayerInputManager>();
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            if (i < PlayerPrefabs.Length)
            {
                manager.playerPrefab = PlayerPrefabs[i];
            }
            manager.JoinPlayer(i);
        }
    }

    public void InstantiatePlayersWithSetDevices()
    {
        NumberOfPlayers = PlayerPrefs.GetInt("Number of Players");
        PlayerInputManager manager = GetComponent<PlayerInputManager>();
        string devicePath = null; 
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            if((devicePath = PlayerPrefs.GetString($"Player_{i}_device", null)) != null)
            {
                int element = PlayerPrefs.GetInt($"Player_{i}_element", 0);
                manager.playerPrefab = PlayerPrefabs[element];
                _players.Add(manager.JoinPlayer(element, pairWithDevice: InputSystem.GetDevice(devicePath)));
            }
        }

        // Begin Countdown
        _countdown = FindObjectOfType<Countdown>();
        _countdown.StartCountdown();
    }

    /// <summary>
    /// Called when a new player input is instantiated
    /// Makes him a child of this gameObject and activate its UI
    /// </summary>
    /// <param name="player"></param>
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetParent(this.transform);
        if (!_indexUI.ContainsKey(player.devices))
        {
            _indexUI.Add(player.devices, player.playerIndex);
            _playerCount++;
        }

        int index = _indexUI[player.devices];
        _playerUIs[index].gameObject.SetActive(true);
        _playerUIs[index].Setup(player.gameObject);

        player.GetComponent<HealthSystem>().onDeath += HandlePlayer_onDeath;
        player.actions["Pause"].started += _pauseScreen.ChangePausedState;
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        int index = _indexUI[player.devices];
        _playerUIs[index].gameObject.SetActive(false);
        _players.Remove(player);
        player.actions["Pause"].started -= _pauseScreen.ChangePausedState;
    }

    public void ActivatePlayersInputs()
    {
        if (_countdown.InCountdown) return; //Do not activate inputs if in countdown
        foreach(PlayerInput player in _players)
        {
            player.ActivateInput();
        }
    }

    public void DeactivatePlayersInputs()
    {
        foreach (PlayerInput player in _players)
        {
            player.DeactivateInput();
        }
    }

    private void HandlePlayer_onDeath()
    {
        _playerCount--;
        if(_playerCount <= 1)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // Deacivate inputs
        DeactivatePlayersInputs();

        //Calculate winner
        int winner = -1;
        foreach (var p in _players)
        {
            if (p.gameObject.activeInHierarchy)
            {
                p.actions["Pause"].started -= _pauseScreen.ChangePausedState;
                winner = p.playerIndex;
                break;
            }
        }

        // Call game over event
        onGameOver?.Invoke(winner);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        foreach (var p in _players)
        {
            p.actions["Pause"].started -= _pauseScreen.ChangePausedState;
        }
    }

}
