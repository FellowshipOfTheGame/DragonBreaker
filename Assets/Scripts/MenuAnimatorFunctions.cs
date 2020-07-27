using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuAnimatorFunctions : MonoBehaviour
{
    public MenuManager MenuStateMachine = null;
    [SerializeField] private GameObject _eventSystem = null;
    public Button buttonToSelect;


    private void Awake()
    {
        MenuStateMachine = FindObjectOfType<MenuManager>();
        _eventSystem = FindObjectOfType<GameObject>();
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

    public void SelectFirstButton()
    {
        buttonToSelect.Select();

        Debug.Log((buttonToSelect.name) + "selected.");
    }
}
