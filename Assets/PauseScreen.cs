using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class PauseScreen : MonoBehaviour
{
    private bool paused = false;

    [SerializeField] private InputActionAsset ActionsAsset = null;
    [SerializeField] private Animator animator = null;

    private void Start()
    {
        ActionsAsset["Pause"].started += ChangePausedState;
        ActionsAsset.Enable();
    }

    public void ChangePausedState(InputAction.CallbackContext obj) => ChangePausedState();

    public void ChangePausedState()
    {
        if (paused) Resume();
        else Pause();

        paused = !paused;
    }

    private void Resume()
    {
        animator.SetBool("PauseStatus", false);
        Time.timeScale = 1f;
        MultiplayerManager.Instance.ActivatePlayersInputs();
    }
    private void Pause()
    {
        animator.SetBool("PauseStatus", true);
        Time.timeScale = 0f;
        MultiplayerManager.Instance.DeactivatePlayersInputs();
        }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        LoadSceneManager.LoadSceneByBuildIndex(0);
    }
}