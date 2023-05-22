using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    public bool canIncreaseTime = true;
    private float timeVal = 0f;

    private static TimerBehavior instance = null;
    public static TimerBehavior Instance { get { return Instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
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
        /*Debug.Log(canIncreaseTime);*/
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0 || sceneIndex == 1)
        {
            Destroy(gameObject);
        }
        if (canIncreaseTime)
        {
            timeVal += Time.unscaledDeltaTime;
        }
        if (GameObject.FindGameObjectWithTag("TimerUI") != null)
        {
            GameObject.FindGameObjectWithTag("TimerUI").GetComponent<Text>().text = timeVal.ToString("f2");
        }
        
    }

    public float GetTime()
    {
        return timeVal;
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }
}
