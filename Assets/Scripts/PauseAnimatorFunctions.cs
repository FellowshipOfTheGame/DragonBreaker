using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseAnimatorFunctions : MonoBehaviour
{
    public Button firstButtonToSelect;

    public void SelectFirstButton()
    {
        firstButtonToSelect.Select();
        Debug.Log((firstButtonToSelect.name) + " selected.");
    }

    public void DeselectButtons()
    {
        GameObject EventSystem = GameObject.Find("EventSystem");
        EventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}