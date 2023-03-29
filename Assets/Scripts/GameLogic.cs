using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public int totalScenes = 7;
    public float holdToRestart = 2.0f;
    [SerializeField] private AudioClip[] loseClips;
    [SerializeField] private AudioClip[] winClips;
    private AudioSource mySource;

    private GameObject restartGUI;
    private Slider restartBar;

    private float holdTimer = 0.0f;
    private GameObject winPanel;
    private GameObject losePanel;
    private bool isLevelFinished = false;
    private bool isWin = false;
    private RotationManager rotationManager;
    private bool isFinishCalled = false;

    private void Start()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        // GUI Stuff
        winPanel = GameObject.FindGameObjectWithTag("WinPanel");
        losePanel = GameObject.FindGameObjectWithTag("LosePanel");
        restartGUI = GameObject.FindGameObjectWithTag("RestartGUI");
        restartBar = GameObject.FindGameObjectWithTag("RestartBar").GetComponent<Slider>();
        mySource = GetComponent<AudioSource>();
        restartBar.value = 0;
        restartBar.maxValue = holdToRestart;

        GameObject tmp = GameObject.FindGameObjectWithTag("RotationManager");
        if (tmp == null)
        {
            tmp = GameObject.FindGameObjectWithTag("OldRotManager");
        }
        rotationManager = tmp.GetComponent<RotationManager>();

        if (rotationManager.startLevelIndex == -1)
        {
            rotationManager.startLevelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        holdTimer = holdToRestart;
        GameObject.FindGameObjectWithTag("Level").GetComponent<ControlScript>().SetRotateAmount(rotationManager.curRotation, rotationManager.totalRotation);
        restartGUI.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            restartGUI.SetActive(true);
            holdTimer -= Time.deltaTime;
            restartBar.value = restartBar.maxValue - holdTimer;
            if (holdTimer < 0)
            {
                Debug.Log("Starting at beginning");
                rotationManager.tag = "OldRotManager";
                SceneManager.LoadScene(rotationManager.startLevelIndex);
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            restartGUI.SetActive(false);
            rotationManager.tag = "OldRotManager";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        

        if (isLevelFinished && isWin)
        {
            if (SceneManager.GetActiveScene().buildIndex < totalScenes)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public bool GetLevelFinished()
    {
        return isLevelFinished;
    }

    public void FinishLevel(bool winStatus)
    {
        isWin = winStatus;
        
        if (!isFinishCalled)
        {
            isFinishCalled = true;
            if (winStatus)
            {
                DoSlowMotion();
                rotationManager.curRotation = GameObject.FindGameObjectWithTag("Level").GetComponent<ControlScript>().GetRotationLeft();
                rotationManager.tag = "OldRotManager";
                winPanel.SetActive(true);
                StartCoroutine(CountDown(3.5f));
                mySource.clip = winClips[Random.Range(0, winClips.Length)];
                mySource.pitch = Random.Range(1.0f, 2.0f);
                mySource.Play();
            }
            else
            {
                losePanel.SetActive(true);
                mySource.clip = loseClips[Random.Range(0, loseClips.Length)];
                mySource.pitch = 0.85f;
                mySource.Play();
            }
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    IEnumerator CountDown(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        isLevelFinished = true;
    }
}
