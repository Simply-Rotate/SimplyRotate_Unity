using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DRMLoginFlowManager : MonoBehaviour
{
    public string scenePathToLoad;

    public GameObject platformSelectUI;

    public GameObject ludareLoginUI;

    public bool dontLoadLevel;

    private GameObject menuInst;

    // Start is called before the first frame update
    void Start()
    {
        LudareManager.onLogin.AddListener(OnLogin);

//if LUDARE_USE_EOS_LOGIN || LUDARE_USE_STEAM_LOGIN || LUDARE_USE_ANDROID_LOGIN
        //menuInst = Instantiate(platformSelectUI, new Vector3(0, 0, 0), Quaternion.identity);
//else
        menuInst = Instantiate(ludareLoginUI, new Vector3(0, 0, 0), Quaternion.identity);
//endif
        DRMSignInScreen signIn = menuInst.GetComponent<DRMSignInScreen>();

        DRMPlatformSelect platformSelect = menuInst.GetComponent<DRMPlatformSelect>();

        if(signIn != null)
        {
            signIn.closeEvent.AddListener(SuperExit);
        }
        
        else if(platformSelect != null)
        {
            platformSelect.closeEvent.AddListener(SuperExit);
        }
    }

    void OnLogin(bool success)
    {
        if(success == true)
        {
            if (dontLoadLevel == false)
            {
                SceneManager.LoadScene(scenePathToLoad);
            }
            else
            {
                PlatformSelect plat = menuInst.GetComponent<PlatformSelect>();
                if(plat != null)
                {
                    plat.CleanupMenu();
                }
                else if(menuInst != null)
                {
                    Destroy(menuInst.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(menuInst == null)
        {
            Application.Quit();
        }
    }

    void SuperExit(bool confirm)
    {
        if(confirm == true)
        {
            Application.Quit();
        }
    }
}
