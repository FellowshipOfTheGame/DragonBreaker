using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverAnimatorFunctions : MonoBehaviour
{
    public Button firstButtonToSelect;

    public void SelectFirstButton()     //selects the first chosen button
    {
        firstButtonToSelect.Select();
    }
}