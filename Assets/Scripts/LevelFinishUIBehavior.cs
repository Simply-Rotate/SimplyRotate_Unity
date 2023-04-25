using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelFinishUIBehavior : MonoBehaviour
{
    [SerializeField] private Text myTimer;
    [SerializeField] private Text myStats;
    private float totalTime;
    private int totalRestarts;
    private void Start()
    {
        if (FindObjectOfType<TimerBehavior>() != null)
        {
            totalTime = FindObjectOfType<TimerBehavior>().GetTime();
        }
        if (FindObjectOfType<StatsBehavior>() != null)
        {
            totalRestarts = FindObjectOfType<StatsBehavior>().GetNumOfRestarts();
        }

        if (totalTime <= 0f)
        {
            myTimer.text = "N/A";
        }
        else
        {
            myTimer.text = totalTime.ToString("f2");
        }
        myStats.text = totalRestarts.ToString();
    }
}
