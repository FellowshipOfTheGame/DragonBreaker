using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.U2D;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject mCamera = null;
    [SerializeField] private Image pCamOn = null;
    [SerializeField] private Image pCamOff = null;
    [SerializeField] private AudioSource pauseSound = null;

private bool _paused = false;

    public void ChangePausedState(InputAction.CallbackContext obj) => ChangePausedState();

    public void ChangePausedState()
    {
        if (_paused) Resume();
        else Pause();

        _paused = !_paused;
    }

    private void Resume()
    {
        if (animator == null)
        {
            Debug.Log($"Animator is null for {gameObject.name}");
        }
        animator.SetBool("PauseStatus", false);
        Time.timeScale = 1f;
        MultiplayerManager.Instance.ActivatePlayersInputs();
        Debug.Log("Resumed.");
    }

    private void Pause()
    {
        if(animator == null)
        {
            Debug.Log($"Animator is null for {gameObject.name}");
        }
        animator.SetBool("PauseStatus", true);
        Time.timeScale = 0f;
        MultiplayerManager.Instance.DeactivatePlayersInputs();
        pauseSound.Play();

    }

    public void CheckPixelCamera()
    {

        if (mCamera.GetComponent<PixelPerfectCamera>().enabled == true)
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