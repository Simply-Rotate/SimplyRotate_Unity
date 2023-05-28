using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pausePanel;
    private TimerBehavior myTimer;
    private GameLogic myLogic;
    private void Start()
    {
        myTimer = FindObjectOfType<TimerBehavior>();
        myLogic = FindObjectOfType<GameLogic>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        if (myTimer != null && myLogic.GetIsLevelFinished())
        {
            myTimer.canIncreaseTime = true;
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        if (myTimer != null)
        {
            myTimer.canIncreaseTime = false;
        }
    }

    public void LevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
