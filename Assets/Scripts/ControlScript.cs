using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    public float rotateSpeed = 10.0f;
    public bool isMenu = false;
    private float totalDegrees = 0.0f;
    public Slider rotateBar;
    private float speedUpFactor = 2.0f;
    public bool canRotate = true;
    private BoxCollider2D myCol;
    public bool isInside = false;
    private float rotateFactor = 0.0f;
    private bool canSpeedUp = false;
    private bool hasTriggeredNormal = false;
    public Material VHSShader;
    private StatsBehavior myStats;

    private void Start()
    {
        myStats = FindObjectOfType<StatsBehavior>();
    }

    public void SetRotateAmount(float curAmount, float maxAmount)
    {
        totalDegrees = curAmount;
        rotateBar.maxValue = maxAmount;
        rotateBar.value = curAmount;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            canSpeedUp = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canSpeedUp = false;
        }
    }

    void FixedUpdate()
    {
        /*float funny = Input.GetAxis("Vertical");*/
        float turn = Input.GetAxis("Horizontal");
        /*transform.Rotate(-funny * rotateSpeed, 0f, -turn * rotateSpeed);*/

        if (myStats != null) myStats.AddRotation(Mathf.Abs(turn));

        if (turn == 0f)
        {
            rotateFactor = 0.0f;
        }

        if (canRotate)
        {
            if (!isMenu)
            {
                rotateBar.value = totalDegrees;
                if (totalDegrees >= 0.0f)
                {
                    transform.Rotate(0f, 0f, -turn * rotateSpeed);
                    rotateFactor += Mathf.Abs(turn) * 0.5f;
                    if (rotateFactor >= 0.95f)
                    {
                        totalDegrees -= Mathf.Abs(turn);
                    }
                    else if (rotateFactor < 0.95f)
                    {
                        totalDegrees -= Mathf.Abs(turn * 0.2f);
                    }
                }
            }
            else
            {
                transform.Rotate(0f, 0f, -turn * rotateSpeed);
            }
        }
        

        if (canSpeedUp)
        {
            DoSpeedUp();
            hasTriggeredNormal = false;
        }
        else if (!canSpeedUp && !hasTriggeredNormal)
        {
            ReturnToNormal();
            hasTriggeredNormal = true;
        }


        // debugging stuff
        /*if (Input.GetKey(KeyCode.Alpha1))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }*/
    }

    public float GetRotationLeft()
    {
        return totalDegrees;
    }

    private void DoSpeedUp()
    {
        Time.timeScale = speedUpFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        VHSShader.SetFloat("_ScanningLines", 5f);
        VHSShader.SetFloat("_ScanningLinesAmount", 0.1f);
        VHSShader.SetFloat("_StaticAmount", 0.2f);
    }

    private void ReturnToNormal()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        VHSShader.SetFloat("_ScanningLines", 1.25f);
        VHSShader.SetFloat("_ScanningLinesAmount", 0.01f);
        VHSShader.SetFloat("_StaticAmount", 0f);
    }
}
