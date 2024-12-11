using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.Events;

public class SignInScreen : MonoBehaviour
{
    [SerializeField]
    private UIDocument signInDoc;

    private TextField passwordField;

    private TextField usernameField;

    private Button exitButton;

    private Toggle rememberCreds;

    public Label message;

    private string hash;

    public LoginCloseEvent onLoginClose;

    public void Awake()
    {
        onLoginClose = new LoginCloseEvent();
    }

    public void Start()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = signInDoc.rootVisualElement;

        Button submitSelect = root.Query<Button>("Submit");
        exitButton = root.Query<Button>("ExitButton");
        usernameField = root.Query<TextField>("UsernameField");
        passwordField = root.Query<TextField>("PasswordField");
        message = root.Query<Label>("StatusText");
        rememberCreds = root.Query<Toggle>("RememberField");

        if(LudareManager.single.credUsername != "")
        {
            usernameField.value = LudareManager.single.credUsername;
        }

        if(LudareManager.single.credHash != "")
        {
            hash = LudareManager.single.credHash;
            rememberCreds.value = true;
        }

        submitSelect.RegisterCallback<ClickEvent>(OnSubmitSelected);
        SetupExit(exitButton);
    }

    public virtual void SetupExit(Button ExitButton)
    {
        ExitButton.RegisterCallback<ClickEvent>(OnExitSelected);
    }

    void OnSubmitSelected(ClickEvent Clicked)
    {
        usernameField.SetEnabled(false);
        passwordField.SetEnabled(false);
        rememberCreds.SetEnabled(false);
        exitButton.SetEnabled(false);

        LudareManager.onLogin.AddListener(OnLoginCallback);

        if(passwordField.value == "password" && hash != "")
        {
            LudareManager.single.TryLoginLudare(usernameField.value, hash, true, rememberCreds.value);
        }

        else
        {
            LudareManager.single.TryLoginLudare(usernameField.value, passwordField.value, false, rememberCreds.value);
        }
    }

    public virtual void OnExitSelected(ClickEvent Clicked)
    {
        Destroy(this.gameObject);
    }

    void OnLoginCallback(bool Success)
    {
        usernameField.SetEnabled(true);
        passwordField.SetEnabled(true);
        rememberCreds.SetEnabled(true);
        exitButton.SetEnabled(true);

        if (Success == true)
        {
            message.text = "Ludare Logged In Successfully";
            StartCoroutine(CloseAfterWait());
            message.style.color = new StyleColor(Color.green);
        }
        else
        {
            message.text = "Ludare Failed to Login";
            message.style.color = new StyleColor(Color.red);
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