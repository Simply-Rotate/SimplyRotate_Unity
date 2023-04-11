using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    [SerializeField] private int lvl; 

    public void LoadThisLevel(int sceneInd)
    {
        
        StartCoroutine(LoadLevel(sceneInd));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadThisLevelByName(int lvl)
    {
        StartCoroutine(LoadLevelByName("Level_" + lvl.ToString()));
    }

    IEnumerator LoadLevelByName(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
