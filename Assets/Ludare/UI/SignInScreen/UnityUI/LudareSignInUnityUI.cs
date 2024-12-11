using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LudareSignInUnityUI : MonoBehaviour
{
    [SerializeField]
    private string password;

    private string username;

    public string message;

    public bool remember;

    public LoginCloseEvent onLoginClose;

    //public InputField usernameField;

    public void Awake()
    {
        onLoginClose = new LoginCloseEvent();
    }

    public void Start()
    {

        /*Button submitSelect = root.Query<Button>("Submit");
        Button exit = root.Query<Button>("ExitButton");
        usernameField = root.Query<TextField>("UsernameField");
        passwordField = root.Query<TextField>("PasswordField");
        message = root.Query<Label>("StatusText");

        submitSelect.RegisterCallback<ClickEvent>(OnSubmitSelected);
        SetupExit(exit);*/
    }

    public virtual void SetupExit()
    {

    }

    public void UpdateUsername(ChangeEvent<string> NewUsername)
    {
        username = NewUsername.newValue;
    }

    public void UpdatePassword(ChangeEvent<string> NewPassword)
    {
        password = NewPassword.newValue;
    }

    public void SetRemember(ChangeEvent<bool> Remember)
    {
        remember = Remember.newValue;
    }

    public void OnSubmitSelected()
    {
        //usernameField.focusable = false;

        LudareManager.onLogin.AddListener(OnLoginCallback);
        //LudareManager.single.TryLoginLudare(username, password, );
    }

    public virtual void OnExitSelected()
    {
        Destroy(this.gameObject);
    }

    void OnLoginCallback(bool Success)
    {

        if (Success == true)
        {
            //message.text = "Ludare Logged In Successfully";
            StartCoroutine(CloseAfterWait());
            //message.style.color = new StyleColor(Color.green);
        }
        else
        {
            //message.text = "Ludare Failed to Login";
            //message.style.color = new StyleColor(Color.red);
        }
    }

    IEnumerator CloseAfterWait()
    {
        yield return new WaitForSeconds(5);

        onLoginClose.Invoke();

        Destroy(gameObject);

        yield return null;
    }
}
