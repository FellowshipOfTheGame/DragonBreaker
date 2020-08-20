using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

public class PauseScreen : MonoBehaviour
{
    private bool paused = false;

    [SerializeField] private InputActionAsset ActionsAsset;
    [SerializeField] private Animator animator;

    private void Start()
    {
        //ActionsAsset["Pause"].started += ChangePausedState;
        //ActionsAsset.Enable();
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
    }
    private void Pause()
    {
        animator.SetBool("PauseStatus", true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        LoadSceneManager.LoadSceneByBuildIndex(0);
    }
}
