using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [Header("Scene Settings")]
    public float slowdownFactor = 0.05f;
    public int totalScenes = 7;
    private float holdToRestart = 2.0f;
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
    private ControlScript myControl;
    private bool canRestart = true;
    private bool curLevelFin = false;
    private GameObject restartIcon;

    /*[Header("Secret Settings DO NOT CHANGE!!!")]
    public bool canTransition = false;
    public bool needsRestart = false;*/

    private void Start()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        // GUI Stuff
        myControl = GameObject.FindGameObjectWithTag("Level").GetComponent<ControlScript>();
       
        if (!myControl.isMenu)
        {
            winPanel = GameObject.FindGameObjectWithTag("WinPanel");
            losePanel = GameObject.FindGameObjectWithTag("LosePanel");
            restartGUI = GameObject.FindGameObjectWithTag("RestartGUI");
            restartIcon = GameObject.FindGameObjectWithTag("RestartIcon");
            mySource = GetComponent<AudioSource>();
            if (GameObject.FindGameObjectWithTag("RestartBar") != null)
            {
                restartBar = GameObject.FindGameObjectWithTag("RestartBar").GetComponent<Slider>();
                restartBar.value = 0;
                restartBar.maxValue = holdToRestart;
            }
            holdTimer = holdToRestart;

            GameObject tmp = GameObject.FindGameObjectWithTag("RotationManager");
            if (tmp == null)
            {
                tmp = GameObject.FindGameObjectWithTag("OldRotManager");
            }
            if (tmp != null)
            {
                rotationManager = tmp.GetComponent<RotationManager>();

                if (rotationManager.startLevelIndex == -1)
                {
                    rotationManager.startLevelIndex = SceneManager.GetActiveScene().buildIndex;
                }
            }
            myControl.SetRotateAmount(rotationManager.curRotation, rotationManager.totalRotation);
            restartGUI.SetActive(false);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }
        
    }

    void Update()
    {
        if (!myControl.isMenu)
        {
            if (canRestart)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    restartGUI.SetActive(true);
                    restartIcon.SetActive(false);
                    holdTimer -= Time.deltaTime;
                    restartBar.value = restartBar.maxValue - holdTimer;
                    if (holdTimer < 0)
                    {
                        Debug.Log("Starting at beginning");
                        rotationManager.tag = "OldRotManager";
                        FindObjectOfType<LevelLoader>().LoadThisLevel(rotationManager.startLevelIndex);
                    }
                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    restartGUI.SetActive(false);
                    rotationManager.tag = "OldRotManager";
                    FindObjectOfType<LevelLoader>().LoadThisLevel(SceneManager.GetActiveScene().buildIndex);
                }
            }
            
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }


            if (isLevelFinished && isWin)
            {
                if (SceneManager.GetActiveScene().buildIndex < totalScenes)
                {
                    /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);*/
                    FindObjectOfType<LevelLoader>().LoadThisLevel(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    FindObjectOfType<LevelLoader>().LoadThisLevel(0);
                }
            }
        }
        
    }

    public bool GetLevelFinished()
    {
        return curLevelFin;
    }

    public void FinishLevel(bool winStatus)
    {
        isWin = winStatus;
        myControl.canRotate = false;
        if (!isFinishCalled)
        {
            isFinishCalled = true;
            if (winStatus)
            {
                curLevelFin = true;
                canRestart = false;
                DoSlowMotion();
                rotationManager.curRotation = myControl.GetRotationLeft();
                rotationManager.tag = "OldRotManager";
                winPanel.SetActive(true);
                StartCoroutine(CountDown(1.5f));
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
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
    }
}
