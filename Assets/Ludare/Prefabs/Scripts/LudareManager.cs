using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

#if LUDARE_USE_STEAM_LOGIN
using Steamworks;
#endif

#if LUDARE_USE_EOS_LOGIN
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using PlayEveryWare.EpicOnlineServices;
#endif

#if LUDARE_USE_ANDROID_LOGIN
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class LoginEvent : UnityEvent<bool> { };

public class LoginCloseEvent : UnityEvent { };

public class DRMCloseEvent : UnityEvent<bool> { };

struct LoginResponse
{
    public bool success;
    public string message;
    public string LudareID;
    public string hash;
}

[Serializable]
struct EventStruct
{
    public string timestamp;
    public string gameid;
    public string userid;
    public string secret;
}

[Serializable]
struct EventFileStruct
{
    public string url;
    public EventStruct message;
}

public class LudareManager : MonoBehaviour
{
    static string appdataDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
    static string appDirectory = "\\Ludare";
    static string backupFileName = appDirectory + "\\LocalEvents.txt";
    static string credFileName = appDirectory + "\\Credentials.txt";
    static string fullDirectory = appdataDirectory + backupFileName;
    static string fullCredDirectory = appdataDirectory + credFileName;

    public string credUsername;
    public string credHash;

    public string ludareUserID;

    public string platformUserID;

    public string ludareGameID;

    public string ludareGameSecret;

    public bool debugOutput;

    public float maxTickTimer = 300;

    public float currTimer = 0;

    static public LudareManager single;

    public static LoginEvent onLogin;

    bool trackingStarted;

    private IEnumerator loginCoroutine;

    private StreamReader localBackupReader;

    private StreamWriter localBackupWriter;

    public NetworkReachability currNetworkState = NetworkReachability.NotReachable;

    public GameObject loadingPrefab;

    public GameObject loadingInst;

#if LUDARE_USE_EOS_LOGIN
    private EOSManager eosManager;

    private GameObject eosManagerObject;

    public GameObject eosManagerPrefab;
#endif

    void Awake()
    {

        onLogin = new LoginEvent();

#if LUDARE_USE_ANDROID_LOGIN
        PlayGamesPlatform.Activate();
#endif

#if LUDARE_USE_EOS_LOGIN
        eosManager = (EOSManager)FindObjectOfType(typeof(EOSManager));

        if(eosManager == null)
        {
            eosManagerObject = Instantiate(eosManagerPrefab, new Vector3(0,0,0), Quaternion.identity);
            eosManager = eosManagerObject.GetComponent<EOSManager>();
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(appdataDirectory);
        if(single != null)
        {
            Destroy(this);
            return;
        }

        single = this;
        currTimer = maxTickTimer;
        DontDestroyOnLoad(gameObject);
        
        if (Directory.Exists(appdataDirectory + appDirectory) == false)
        {
            return;
        }

        if (File.Exists(fullCredDirectory) == false)
        {
            return;
        }

        StreamReader CredReader = File.OpenText(fullCredDirectory);
        string TmpReader;

        if((TmpReader = CredReader.ReadLine()) != null)
        {
            credUsername = TmpReader;

            if((TmpReader = CredReader.ReadLine()) != null)
            {
                credHash = TmpReader;
            }
        }

        CredReader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        currTimer -= Time.deltaTime;

        if (currTimer <= 0)
        {

            StartCoroutine(UpdateTracking());

            currTimer = maxTickTimer;
        }
    }

    void GameStart()
    {

    }

    void OnApplicationQuit()
    {
        StartCoroutine(TryWriteEvent("/RegisterGameShutdown"));
    }

    public bool TryLoginPlatform()
    {

        StartLoading();
#if LUDARE_USE_STEAM_LOGIN
        platformUserID = SteamUser.GetSteamID().ToString();

        StartCoroutine(TryLoginPlatform("Steam", platformUserID));
#endif

#if LUDARE_USE_EOS_LOGIN
        EOSManager.Instance.StartLoginWithLoginTypeAndToken(LoginCredentialType.AccountPortal, "", "", StartLoginWithLoginTypeAndTokenCallback);
#endif

#if LUDARE_USE_ANDROID_LOGIN
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if(success == SignInStatus.Success)
            {
                if(debugOutput == true) Debug.Log("Login Success");

            }
        });
#endif

        return false;
    }

    public void TryLoginLudare(string Username, string Password, bool Hash, bool Remember)
    {
        StartLoading();
        StartCoroutine(TryLogin(Username, Password, Hash, Remember));

    }

#if LUDARE_USE_EOS_LOGIN
    public void StartLoginWithLoginTypeAndTokenCallback(Epic.OnlineServices.Auth.LoginCallbackInfo loginCallbackInfo)
    {
        if(loginCallbackInfo.ResultCode == Result.Success)
        {
            platformUserID = loginCallbackInfo.LocalUserId.ToString();
            if(debugOutput == true) Debug.Log("Login Success");

            StartCoroutine(TryLoginPlatform("Epic", platformUserID));
        }
        else
        {
            if(debugOutput == true) Debug.Log("Login failed");
        }
    }
#endif

    IEnumerator CreateAndAccessBackupFile(bool read)
    {
        if (Directory.Exists(appdataDirectory + appDirectory) == false)
        {
            Directory.CreateDirectory(appdataDirectory + appDirectory);
        }

        if (File.Exists(fullDirectory) == false)
        {
            File.Create(fullDirectory).Close();
        }

        while (localBackupWriter != null || localBackupReader != null)
        {
            yield return new WaitForSeconds(1);
        }

        if (read == true)
        {
            localBackupReader = File.OpenText(fullDirectory);
        }
        else
        {
            localBackupWriter = File.AppendText(fullDirectory);
        }
    }

    public void StartLoading()
    {
        loadingInst = Instantiate(loadingPrefab, Vector3.zero, Quaternion.identity);
    }

    public void EndLoading()
    {
        Destroy(loadingInst);
    }

    IEnumerator TryWriteEvent(string eventType)
    {
        if (currNetworkState == NetworkReachability.NotReachable && Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(UploadBacklog());
        }

        currNetworkState = Application.internetReachability;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(WriteToFile("https://www.devpowered.com:8000" + eventType));
        }
        else
        {

            UnityWebRequest www = UnityWebRequest.Post("https://www.devpowered.com:8000" + eventType,
                "{ \"userid\": \"" + ludareUserID + "\", \"gameid\": \"" + ludareGameID + "\", \"secret\": \"" + ludareGameSecret + "\", \"timestamp\": \"" + System.DateTime.Now + "\" }",
                "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {

                StartCoroutine(WriteToFile("https://www.devpowered.com:8000" + eventType));
            }
        }

        yield return null;
    }

    IEnumerator WriteToFile(string URL)
    {
        EventFileStruct Store = new EventFileStruct();
        Store.url = URL;

        EventStruct Msg = new EventStruct();
        Msg.gameid = ludareGameID;
        Msg.userid = ludareUserID;
        Msg.timestamp = System.DateTime.Now.ToString();
        Msg.secret = ludareGameSecret;

        Store.message = Msg;

        yield return CreateAndAccessBackupFile(false);

        string JsonStr = JsonUtility.ToJson(Store);

        localBackupWriter.WriteLine(JsonStr);

        localBackupWriter.Close();
        localBackupWriter = null;

    }

    IEnumerator TryLogin(string Username, string Password, bool Hash, bool Remember)
    {
        UnityWebRequest www = UnityWebRequest.Post("https://www.devpowered.com:8000/GameLogin",
            "{ \"password\": \"" + Password + "\", \"hashed\": \"" + Hash + "\", \"secret\": \"" + ludareGameSecret + "\", \"returnHash\": \"" + Remember + "\", \"username\": \"" + Username + "\" }",
            "application/json");

        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            if (debugOutput == true)
            {
                Debug.Log("Login failed");
                Debug.Log(www.error);
            }
        }
        else
        {

            LoginResponse res = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

            onLogin.Invoke(res.success);

            if (res.success == true)
            {
                ludareUserID = res.message;

                StartCoroutine(StartTracking());

                if(Remember == true)
                {
                    if (Directory.Exists(appdataDirectory + appDirectory) == false)
                    {
                        Directory.CreateDirectory(appdataDirectory + appDirectory);
                    }

                    if (File.Exists(fullCredDirectory) == false)
                    {
                       File.Create(fullCredDirectory).Close();
                    }

                    StreamWriter CredWriter = File.CreateText(fullCredDirectory);

                    CredWriter.WriteLine(Username);
                    CredWriter.WriteLine(res.hash);

                    CredWriter.Close();
                }
            }
        }

        EndLoading();
    }

    IEnumerator TryLoginPlatform(string Platform, string PlatformId)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.devpowered.com:8000/GetUserID?platform=" + Platform + "&userid=" + PlatformId + "&secret=" + ludareGameSecret);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            if (debugOutput == true)
            {
                Debug.Log("Login failed");
                Debug.Log(www.error);
            }
        }
        else
        {

            LoginResponse res = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);

            onLogin.Invoke(res.success);

            if (res.success == true)
            {
                ludareUserID = res.LudareID;

                StartCoroutine(StartTracking());
            }
        }

        EndLoading();
    }

    IEnumerator StartTracking()
    {

        StartCoroutine(TryWriteEvent("/RegisterGameStart"));
        trackingStarted = true;

        yield return null;

    }

    IEnumerator UpdateTracking()
    {
        StartCoroutine(TryWriteEvent("/RegisterGameContinue"));

        yield return null;
    }

    IEnumerator UploadBacklog()
    {
        string JsonStr, nextLine = "";

        yield return CreateAndAccessBackupFile(true);

        while ((nextLine = localBackupReader.ReadLine()) != null)
        {
            EventFileStruct EventMsg = JsonUtility.FromJson<EventFileStruct>(nextLine);

            UnityWebRequest www = UnityWebRequest.Post(EventMsg.url,
                "{ \"userid\": \"" + EventMsg.message.userid + "\", \"gameid\": \"" + EventMsg.message.gameid + "\", \"secret\": \"" + EventMsg.message.secret + "\", \"timestamp\": \"" + EventMsg.message.timestamp + "\" }",
                "application/json");

            yield return www.SendWebRequest();
        }

        localBackupReader.Close();
        localBackupReader = null;

        if(File.Exists(fullDirectory) == true)
        {
            File.Delete(fullDirectory);
        }

        yield return null;
    }
}
