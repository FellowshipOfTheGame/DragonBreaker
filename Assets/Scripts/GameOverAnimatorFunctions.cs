using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverAnimatorFunctions : MonoBehaviour
{
    public Button firstButtonToSelect;

    public void SelectFirstButton()
    {
        firstButtonToSelect.Select();

        Debug.Log((firstButtonToSelect.name) + " selected.");
    }
}