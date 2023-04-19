using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    public bool canIncreaseTime = true;
    private float timeVal = 0f;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (canIncreaseTime)
        {
            timeVal += Time.unscaledDeltaTime;
        }
        GameObject.FindGameObjectWithTag("TimerUI").GetComponent<Text>().text = timeVal.ToString("f2");
    }
}
