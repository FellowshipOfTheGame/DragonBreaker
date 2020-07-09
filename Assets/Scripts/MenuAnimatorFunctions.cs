using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimatorFunctions : MonoBehaviour
{
    public MenuManager MenuStateMachine = null;

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

}
