using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public bool InCountdown => _index < Numbers.Length;

    [SerializeField] private Image CountdownImage = null;
    [SerializeField] private Sprite[] Numbers = null;

    private int _index = 0;
    private const float _countdownTime = 1f;
    
    public void StartCountdown()
    {
        _index = 0;
        MultiplayerManager.Instance.DeactivatePlayersInputs();
        Count();
    }

    private void Count()
    {
        if (InCountdown)
        {
            CountdownImage.sprite = Numbers[_index];
            CountdownImage.transform.localScale = Vector3.one * 2f;
            StartCoroutine("ChangeCountdownScale");
            _index++;
            Invoke("Count", _countdownTime);
        }
        else
        {
            MultiplayerManager.Instance.ActivatePlayersInputs();
        }
    }


    private IEnumerator ChangeCountdownScale()
    {
        float preScale = CountdownImage.transform.localScale.x;
        float elapsedTime = 0f;
        while(elapsedTime < _countdownTime)
        {
            elapsedTime += Time.deltaTime;
            CountdownImage.transform.localScale =  Vector3.one * Mathf.Lerp(preScale, 1, elapsedTime / _countdownTime);
            yield return null;
        }
        CountdownImage.transform.localScale = Vector3.one;
    }
}
