using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.U2D;

public class PauseScreen : MonoBehaviour
{
    private bool paused = false;

    [SerializeField] private InputActionAsset ActionsAsset = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject mCamera = null;
    [SerializeField] private Image pCamOn = null;
    [SerializeField] private Image pCamOff = null;

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

        if (!mCamera.GetComponent<PixelPerfectCamera>().enabled)
        {
            pCamOn.enabled = true;
            pCamOff.enabled = false;
        }
        else
        {
            pCamOn.enabled = false;
            pCamOff.enabled = true;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        LoadSceneManager.LoadSceneByBuildIndex(0);
    }

    public void TogglePixelCamera()
    {
        if (!mCamera.GetComponent<PixelPerfectCamera>().enabled)
        {
            mCamera.GetComponent<PixelPerfectCamera>().enabled = true;
            pCamOn.enabled = true;
            pCamOff.enabled = false;
            Debug.Log("Toggled Pixel Camera ON");
        }
        else
        {
            mCamera.GetComponent<PixelPerfectCamera>().enabled = false;
            pCamOn.enabled = false;
            pCamOff.enabled = true;
            Debug.Log("Toggled Pixel Camera OFF");
        }
    }
}