using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private bool canShowHint = true;
    private bool isSpeedRun = false;
    private static GameSettings instance = null;
    public static GameSettings Instance { get { return Instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Debug.Log(canShowHint);
    }

    public void ShowHint(bool flag)
    {
        canShowHint = flag;
    }

    public bool GetCanShowHint()
    {
        return canShowHint;
    }

    public void SetSpeedRun(bool flag)
    {
        isSpeedRun = flag;
    }

    public bool GetIsSpeedRun()
    {
        return isSpeedRun;
    }
}
