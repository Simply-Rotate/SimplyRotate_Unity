using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

public class DRMPlatformSelect : PlatformSelect
{
    public GameObject ExitConfirmMenu;

    public DRMCloseEvent closeEvent;

    public void Awake()
    {
        closeEvent = new DRMCloseEvent();
    }

    public override void SetExit(Button exitButton)
    {
        exitButton.RegisterCallback<ClickEvent>(OnExitClicked);
    }

    public override void OnExitClicked(ClickEvent Clicked)
    {
        GameObject exitInst = Instantiate(ExitConfirmMenu, new Vector3(0, 0, 0), Quaternion.identity);

        DRMExitOutMenu menu = exitInst.GetComponent<DRMExitOutMenu>();

        if(menu != null)
        {
            menu.closeEvent.AddListener(SuperExit);
        }

        this.enabled = false;
    }

    void SuperExit(bool confirm)
    {
        this.enabled = true;

        closeEvent.Invoke(confirm);
    }
}
