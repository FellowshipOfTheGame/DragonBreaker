using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitTimeFilling : MonoBehaviour
{
    private Image _image;
    private int count = 0;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnExitFillStart()
    {
        count++;
    }

    public void OnExitFillStop()
    {
        count--;
    }

    public void OnExitFillReset()
    {
        count = 0;
    }

    private void Update()
    {
        if (count >= 1)
        {
            _image.fillAmount += Time.deltaTime;
        }
        else
        {
            count = 0;
            _image.fillAmount = 0f;
        }
    }

}
