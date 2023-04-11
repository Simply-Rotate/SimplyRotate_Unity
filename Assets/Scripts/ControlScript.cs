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
    private float speedUpFactor = 3.0f;
    public bool canRotate = true;
    private Rigidbody2D rb;
    private Vector3 myDir;
    private BoxCollider2D myCol;
    private bool isInside = false;
    private float rotateFactor = 0.0f;
    private bool canSpeedUp = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCol = GetComponent<BoxCollider2D>();
        Debug.Log(myCol);
    }

    public void SetRotateAmount(float curAmount, float maxAmount)
    {
        totalDegrees = curAmount;
        rotateBar.maxValue = maxAmount;
        rotateBar.value = curAmount;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (myCol != null)
        {
            if (other.gameObject != null && isInside)
            {
                isInside = false;
                Debug.Log("Out");
                other.gameObject.transform.SetParent(null, true);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (myCol != null)
        {
            if (other.gameObject != null && myCol.IsTouching(other))
            {
                other.gameObject.transform.SetParent(transform, true);
                isInside = true;
            }
        }
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
        }
        else if (!canSpeedUp)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
        }
        

        // debugging stuff
        if (Input.GetKey(KeyCode.Alpha1))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(17);
        }
        /*if (Input.GetKey(KeyCode.Alpha2))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(2);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(5);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(7);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(10);
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
    }
}
