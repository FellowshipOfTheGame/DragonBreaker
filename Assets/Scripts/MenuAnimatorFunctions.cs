using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorFunctions : MonoBehaviour
{
    public MenuManager MenuStateMachine = null;
    [SerializeField] private GameObject _eventSystem = null;


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

    public void ResetEventSystem()
    {
        _eventSystem.SetActive(false);
        _eventSystem.SetActive(true);
        Debug.Log("EventSystem restarted.");
    }
}
