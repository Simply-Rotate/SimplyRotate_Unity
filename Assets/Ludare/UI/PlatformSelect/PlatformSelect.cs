using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

public class PlatformSelect : MonoBehaviour
{
    enum LudarePlatforms
    {
        Epic = 0,
        Steam = 1,
        Android = 2,
    }

    [SerializeField]
    private UIDocument platformDoc;

    [SerializeField]
    private GameObject ludareLoginPrefab;

    private GameObject loginInst;

    [SerializeField]
    public Texture2D[] platformIcons;

    public Label message;

    public void Start()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = platformDoc.rootVisualElement;

        Button platformSelect = root.Query<Button>("PlatformButton");
        Button ludareSelect = root.Query<Button>("LudareButton");
        Button exitButton = root.Query<Button>("ExitButton");
        Label platformLabel = platformSelect.Query<Label>("PlatformName");
        VisualElement platformIcon = platformSelect.Query<VisualElement>("PlatformIcon");
        message = root.Query<Label>("StatusText");

#if LUDARE_USE_EOS_LOGIN
        platformLabel.text = "Epic";
        platformIcon.style.backgroundImage = Background.FromTexture2D(platformIcons[(int)LudarePlatforms.Epic]);
#endif

#if LUDARE_USE_STEAM_LOGIN
        platformLabel.text = "Steam";
        platformIcon.style.backgroundImage = Background.FromTexture2D(platformIcons[(int)LudarePlatforms.Steam]);
#endif

#if LUDARE_USE_ANDROID_LOGIN
        platformLabel.text = "Android";
        platformIcon.style.backgroundImage = Background.FromTexture2D(platformIcons[(int)LudarePlatforms.Android]);
#endif

        platformSelect.RegisterCallback<ClickEvent>(OnPlatformClicked);
        ludareSelect.RegisterCallback<ClickEvent>(OnLudareClicked);
        SetExit(exitButton);
    }

    public virtual void SetExit(Button exitButton)
    {
        Debug.Log("base");
        exitButton.RegisterCallback<ClickEvent>(OnExitClicked);
    }

    void OnPlatformClicked(ClickEvent Clicked)
    {
        LudareManager.onLogin.AddListener(OnLoginCallback);
        LudareManager.single.TryLoginPlatform();
    }

    void OnLudareClicked(ClickEvent Clicked)
    {
        loginInst = Instantiate(ludareLoginPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        SignInScreen signIn = loginInst.GetComponent<SignInScreen>();

        if (signIn != null)
        {
            signIn.onLoginClose.AddListener(SuperMenuClose);
        }

        loginInst.transform.SetParent(this.transform.parent);
    }

    void SuperMenuClose()
    {
        Destroy(gameObject);
    }

    public virtual void OnExitClicked(ClickEvent Clicked)
    {
        Destroy(gameObject);
    }

    void OnLoginCallback(bool Success)
    {

        if(Success == true)
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

        Destroy(gameObject);

        yield return null;
    }

    public void CleanupMenu()
    {
        if (loginInst != null)
        {
            Destroy(loginInst.gameObject);
        }
        Destroy(this.gameObject);
    }
}