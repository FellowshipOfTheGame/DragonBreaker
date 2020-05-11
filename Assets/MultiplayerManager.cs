using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
    public event Action<int> onGameOver;

    [SerializeField] private PlayerUI[] _playerUIs = { };
    [SerializeField] private int _playerCount = 0;

    private Dictionary<ReadOnlyArray<InputDevice>, int> _indexUI;
    private List<PlayerInput> _players;
    private int _alivePlayerCount = 0;

    /// <summary>
    /// Called when a new player input is instantiated
    /// Makes him a child of this gameObject and activate its UI
    /// </summary>
    /// <param name="player"></param>
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetParent(this.transform);
        _players.Add(player);
        if (!_indexUI.ContainsKey(player.devices))
        {
            _indexUI.Add(player.devices, _playerCount);
            _playerCount++;
        }

        int index = _indexUI[player.devices];
        _playerUIs[index].gameObject.SetActive(true);
        _playerUIs[index].Setup(player.gameObject);

        player.GetComponent<HealthSystem>().onDeath += HandlePlayerJoin_onDeath;

        _alivePlayerCount++;
    }

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
        _alivePlayerCount = 0;
        _players = new List<PlayerInput>();
        _indexUI = new Dictionary<ReadOnlyArray<InputDevice>, int>();
    }

    private void HandlePlayerJoin_onDeath()
    {
        _alivePlayerCount--;
        if(_alivePlayerCount <= 1)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        //Calculate winner
        int winner = 0;
        foreach (var p in _players)
        {
            if (p.gameObject.activeInHierarchy)
            {
                winner = p.playerIndex;
                break;
            }
        }

        onGameOver?.Invoke(winner);
        //Reload active Scene
        Invoke("ReloadScene", 1f);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
