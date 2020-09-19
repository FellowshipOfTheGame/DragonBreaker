using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class MenuStateManager : MonoBehaviour
{
    [System.Serializable]
    public struct MenuState {
        public string StateName;
        public UnityEvent OnEnterState;
        public UnityEvent OnExitState;
    };

    protected Dictionary<string, MenuState> states = new Dictionary<string, MenuState>();
    protected MenuState _actualState;

    public virtual void ChangeState(string stateName)
    {
        //Check if state name is valid
        if (states.ContainsKey(stateName))
        {
            //Invoke Exit State Event
            _actualState.OnExitState?.Invoke();
            //Debug.Log($"Exited: {_actualState.StateName}");
            //Change actual state
            _actualState = states[stateName];
            //Invoke Enter State Event
            _actualState.OnEnterState?.Invoke();
            //Debug.Log($"Entered: {_actualState.StateName}");
        }
        else
        {
            //Send warning message
            Debug.LogWarning($"Trying to access unexisting menu state: {stateName}.");
        }
    }

    public virtual void ChangeState(MenuState state)
    {
        //Check if state name is valid
        if (states.ContainsValue(state))
        {
            //Invoke Exit State Event
            _actualState.OnExitState?.Invoke();
            //Debug.Log($"Exited: {_actualState.StateName}");
            //Change actual state
            _actualState = state;
            //Invoke Enter State Event
            _actualState.OnEnterState?.Invoke();
            //Debug.Log($"Entered: {_actualState.StateName}");
        }
        else
        {
            //Send warning message
            Debug.LogWarning($"Trying to access unexisting menu state: {state.StateName}.");
        }
    }

}
