using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelFinishUIBehavior : MonoBehaviour
{
    [SerializeField] private Text myTimer;
    [SerializeField] private Text myRestart;
    [SerializeField] private Text myRotation;
    private float totalTime;
    private int totalRestarts;
    private float totalRotation;
    private void Start()
    {
        if (FindObjectOfType<TimerBehavior>() != null)
        {
            totalTime = FindObjectOfType<TimerBehavior>().GetTime();
            if (totalTime <= 0f)
            {
                myTimer.text = "N/A";
            }
            else
            {
                myTimer.text = totalTime.ToString("f2");
            }
        }
        if (FindObjectOfType<StatsBehavior>() != null)
        {
            totalRestarts = FindObjectOfType<StatsBehavior>().GetNumOfRestarts();
            totalRotation = FindObjectOfType<StatsBehavior>().GetTotalRotation();
            myRestart.text = totalRestarts.ToString();
            myRotation.text = totalRotation.ToString("f2") + " Degrees";
        }
        
    }
}
