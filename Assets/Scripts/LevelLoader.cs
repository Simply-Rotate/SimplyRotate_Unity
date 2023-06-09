using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private Animator transition;
    public float transitionTime = 1f;
    private RotationManager rotationManager;

    [SerializeField] private int lvl;

    private void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("RotationManager");
        if (tmp != null)
        {
            rotationManager = tmp.GetComponent<RotationManager>();

            if (rotationManager.startLevelIndex == -1)
            {
                rotationManager.startLevelIndex = SceneManager.GetActiveScene().buildIndex;
            }
        }
        transition = GameObject.FindGameObjectWithTag("UI").GetComponent<Animator>();
    }

    public void LoadThisLevel(int sceneInd)
    {
        StartCoroutine(LoadLevel(sceneInd));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    /*public void LoadThisLevelByName(int lvl)
    {
        if (rotationManager != null) rotationManager.gameObject.tag = "OldRotManager";
        StartCoroutine(LoadLevelByName("Level_" + lvl.ToString()));
    }

    IEnumerator LoadLevelByName(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(sceneName);
    }*/
}
