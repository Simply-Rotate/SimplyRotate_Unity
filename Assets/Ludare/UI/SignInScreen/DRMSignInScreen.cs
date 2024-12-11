using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DRMSignInScreen : SignInScreen
{
    public GameObject ConfirmClose;

    public DRMCloseEvent closeEvent;

    public void Awake()
    {
        closeEvent = new DRMCloseEvent();
    }

    public override void OnExitSelected(ClickEvent Clicked)
    {
        GameObject obj = Instantiate(ConfirmClose, new Vector3(0, 0, 0), Quaternion.identity);

        DRMExitOutMenu menu = obj.GetComponent<DRMExitOutMenu>();

        if(menu != null)
        {
            menu.closeEvent.AddListener(SuperExit);
        }
    }

    public void SuperExit(bool confirm)
    {
        closeEvent.Invoke(confirm);

        if(confirm == true)
        {
            Destroy(this);
        }
    }
}
