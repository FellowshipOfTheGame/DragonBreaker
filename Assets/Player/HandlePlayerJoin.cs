using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerJoin : MonoBehaviour
{
    [SerializeField] private PlayerUI[] playerUIs = { };
    [SerializeField] private int num_players = 0;

    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetParent(this.transform);
        playerUIs[num_players].gameObject.SetActive(true);
        playerUIs[num_players].Setup(player.gameObject);
        player.GetComponent<HealthSystem>().onDeath += HandlePlayerJoin_onDeath;
        num_players++;
    }

    private void HandlePlayerJoin_onDeath()
    {
        num_players--;
    }
}
