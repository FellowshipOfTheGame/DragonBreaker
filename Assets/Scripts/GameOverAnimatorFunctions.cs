using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverAnimatorFunctions : MonoBehaviour
{
    public Button firstButtonToSelect;
    public AudioSource bgMusic;
    public AudioSource gameOverSound;
    public AudioSource gameOverMusic;


    public void SelectFirstButton()     //selects the first chosen button
    {
        firstButtonToSelect.Select();
    }

    public void GameOverSound()        //stops game music and plays "HUH-HAH" on game over
    {
        bgMusic.Stop();
        gameOverSound.Play();
    }

    public void GameOverMusic()       //plays game over music
    {
        gameOverMusic.Play();
    }
}