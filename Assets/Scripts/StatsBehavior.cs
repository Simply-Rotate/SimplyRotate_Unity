using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StatsBehavior : MonoBehaviour
{
    private int numOfRestarts = 0;
    private float totalRotation = 0f;
    private static StatsBehavior instance = null;
    public static StatsBehavior Instance { get { return Instance; } }
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
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0 || sceneIndex == 1)
        {
            Destroy(gameObject);
        }
    }

    public void AddRestart()
    {
        numOfRestarts += 1;
    }

    public void AddRotation(float amount)
    {
        totalRotation += amount;
    }

    public int GetNumOfRestarts()
    {
        return numOfRestarts;
    }

    public float GetTotalRotation()
    {
        return totalRotation;
    }
}
