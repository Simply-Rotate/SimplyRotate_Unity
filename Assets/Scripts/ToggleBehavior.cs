using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBehavior : MonoBehaviour
{
    public int toggleIdex = -1;
    private Toggle myToggle;
    private void Start()
    {
        myToggle = GetComponent<Toggle>();
    }
    private void Update()
    {
        if (toggleIdex == 0)
        {
            if (myToggle.isOn)
            {
                FindObjectOfType<GameSettings>().ShowHint(true);
            }
            else
            {
                FindObjectOfType<GameSettings>().ShowHint(false);
            }
        }
        else if (toggleIdex == 1)
        {
            if (myToggle.isOn)
            {
                FindObjectOfType<GameSettings>().SetSpeedRun(true);
            }
            else
            {
                FindObjectOfType<GameSettings>().SetSpeedRun(false);
            }
        }
    }

    
}
