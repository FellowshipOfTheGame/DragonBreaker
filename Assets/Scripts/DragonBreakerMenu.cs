using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DragonBreakerMenu : MenuStateManager
{
    [SerializeField] protected InputActionAsset ActionsAsset;
    [SerializeField] protected MenuState[] MenuStates;

    protected void Awake()
    {
        //Populate possible states dictionary
        PopulateMenuDictionary();
        //Start on first State
        ChangeState(MenuStates[0]);
        ActionsAsset["Submit"].canceled += TransitionToMain;
        ActionsAsset["Click"].canceled += TransitionToMain;
        ActionsAsset["Cancel"].canceled += TransitionBackToPressPlay;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadCharacterSelectionScene()
    {
        SceneManager.LoadScene(1);
    }

    private void TransitionBackToPressPlay(InputAction.CallbackContext obj)
    {
        //If state is Main Menu, go back to Press Play to Begin
        if(_actualState.StateName == MenuStates[1].StateName)
        {
            ChangeState(MenuStates[0]); 
        }
        //Else if state is Configurations, go back to Main Menu
        else if (_actualState.StateName == MenuStates[3].StateName)
        {
            ChangeState(MenuStates[1]);
        }
        //Else if state is Credits, go back to Main Menu
        else if (_actualState.StateName == MenuStates[4].StateName)
        { 
            ChangeState(MenuStates[1]);
        }
    }

    protected void PopulateMenuDictionary()
    {
        foreach(var state in MenuStates)
        {
            states.Add(state.StateName, state);
        }
    }

    private void TransitionToMain(InputAction.CallbackContext obj)
    {
        if(_actualState.StateName == MenuStates[0].StateName)
        {
            //Change State
            ChangeState(MenuStates[1]);
        }
    }

    private void OnDestroy()
    {
        ActionsAsset["Submit"].canceled -= TransitionToMain;
        ActionsAsset["Click"].canceled -= TransitionToMain;
        ActionsAsset["Cancel"].canceled -= TransitionBackToPressPlay;
    }



}
