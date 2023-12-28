using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RotationManager : MonoBehaviour
{
    /*public float totalRotation = 200.0f;*/
    public float curRotation = 200.0f;
    public int startLevelIndex = -1;
    public Stack<float> previousRotations;
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
        instance = this;
        previousRotations = new Stack<float>();
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
        curRotation = Mathf.Clamp(curRotation, 0.0f, totalRotation);
        FindObjectOfType<ControlScript>().SetRotateAmount(curRotation, totalRotation);
    }
}
