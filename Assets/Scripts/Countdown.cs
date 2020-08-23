using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource countdownSound;
    public AudioSource finalCountdownSound;

    public bool InCountdown;

    public void StartCountdown()
    {
        MultiplayerManager.Instance.DeactivatePlayersInputs();      //input disabled during countdown
        InCountdown = true;
    }
    public void Count()    //play countdown sound
    {
        countdownSound.Play();
    }

    public void FinalCount()    //play the last countdown sound (BREAK)
    {
        finalCountdownSound.Play();
    }

    public void EndCountdown()  //input and music re-enabled as countdown ends
    {
        backgroundMusic.Play(); 
        MultiplayerManager.Instance.ActivatePlayersInputs();
        InCountdown = false;
    }

}

