using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintToggle : MonoBehaviour
{
    private void Update()
    {
        bool hintSettings = GetComponent<Toggle>().isOn;
        FindObjectOfType<GameSettings>().ShowHint(hintSettings);
    }
}
