using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseAnimatorFunctions : MonoBehaviour
{
    public Button firstButtonToSelect;

    public void SelectFirstButton() //selects the first chosen button
    {
        firstButtonToSelect.Select();
    }

    public void DeselectButtons()   //deselects buttons to prevent sprites from getting stuck
    {
        GameObject EventSystem = GameObject.Find("EventSystem");
        EventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}