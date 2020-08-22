using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAnimatorFunctions : MonoBehaviour
{
    public MenuManager MenuStateMachine = null;
    public Button firstButtonToSelect;

    private void Awake()
    {
        MenuStateMachine = FindObjectOfType<MenuManager>();
    }

    public void ChangeState(int state)
    {
        MenuStateMachine.ChangeState((MenuManager.States)state);
    }

    public void SetIsChanging(int value)
    {
        MenuStateMachine.IsChangingState = value == 0 ? false : true;
    }

    public void PlayEnterStateAnimation()
    {
        MenuStateMachine.PlayEnterStateAnimation();
    }

    public void SelectFirstButton()    //selects the first chosen button
    {
        firstButtonToSelect.Select();
    }

    public void DeselectButtons()   //deselects buttons to prevent sprites from getting stuck
    {
        GameObject EventSystem = GameObject.Find("EventSystem");
        EventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }

}
