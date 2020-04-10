using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerJoin : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput player)
    {
        player.transform.SetParent(this.transform);
    }
}
