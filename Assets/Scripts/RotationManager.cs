using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RotationManager : MonoBehaviour
{
    /*public float totalRotation = 200.0f;*/
    public float curRotation = 200.0f;
    public int startLevelIndex = -1;
    public Stack<float> previousRotations = new Stack<float>();
    public List<int> levelRotationAmount;
    private static RotationManager instance = null;
    public static RotationManager Instance { get { return Instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
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

    public void AddRotation(float amount)
    {
        curRotation += amount;
        float totalRotation = levelRotationAmount[FindObjectOfType<GameLogic>().curLevel];
        if (curRotation > totalRotation) curRotation = totalRotation;
        FindObjectOfType<ControlScript>().SetRotateAmount(curRotation, totalRotation);
    }
}
