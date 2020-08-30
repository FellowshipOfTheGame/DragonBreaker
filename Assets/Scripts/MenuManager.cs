using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public enum States { StartScreen = 0, MainScreen = 1, Options = 2, Credits = 3, CharacterSelection = 4};

    public bool IsChangingState = false;

    public Animator[] animators;
    public States CurrentState = States.StartScreen;
    //public int State = 0;

    [SerializeField] protected InputActionAsset ActionsAsset;

    public AudioSource backSound;

    protected void Awake()
    {
        ActionsAsset["Submit"].canceled += TransitionToMain;
        ActionsAsset["Click"].canceled += TransitionToMain;
        ActionsAsset["Cancel"].canceled += TransitionBack;
    }

    protected void Start() => PlayEnterStateAnimation();


    public void ChangeState(States state)
    {
        if (!IsChangingState)
        {
            IsChangingState = true;
            animators[(int)CurrentState].SetTrigger("Exit");
            CurrentState = state;
        }
        else
        {
            //Debug.Log($"In transition from state {CurrentState}, cannot change to {state}");
        }
    }

    public void PlayEnterStateAnimation()
    {
        //Debug.Log($"Setting trigger Enter of {CurrentState}");
        animators[(int)CurrentState].SetTrigger("Enter");
    }

    private void TransitionToMain(InputAction.CallbackContext obj)
    {
        if (CurrentState.Equals(States.StartScreen))
        {
            ChangeState(States.MainScreen);
        }
    }

    private void TransitionBack(InputAction.CallbackContext obj)
    {
        if (CurrentState.Equals(States.MainScreen))
        {
            ChangeState(States.StartScreen);
            backSound.Play();
        }
        else if (CurrentState.Equals(States.Options) || CurrentState.Equals(States.Credits))
        {
            ChangeState(States.MainScreen);
            backSound.Play();
        }
    }

    private void OnDestroy()
    {
        ActionsAsset["Submit"].canceled -= TransitionToMain;
        ActionsAsset["Click"].canceled -= TransitionToMain;
        ActionsAsset["Cancel"].canceled -= TransitionBack;
    }



}
