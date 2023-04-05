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

    private Rigidbody2D rb;
    private Vector3 myDir;
    private BoxCollider2D myCol;
    private bool isInside = false;

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

    void FixedUpdate()
    {
        /*float funny = Input.GetAxis("Vertical");*/
        float turn = Input.GetAxis("Horizontal");
        /*transform.Rotate(-funny * rotateSpeed, 0f, -turn * rotateSpeed);*/
        
        if (!isMenu)
        {
            rotateBar.value = totalDegrees;
            if (totalDegrees >= 0.0f)
            {
                transform.Rotate(0f, 0f, -turn * rotateSpeed);
                totalDegrees -= Mathf.Abs(turn);
            }
        }
        else
        {
            transform.Rotate(0f, 0f, -turn * rotateSpeed);
        }
        

        // debugging stuff
        if (Input.GetKey(KeyCode.Alpha1))
        {
            FindObjectOfType<RotationManager>().tag = "OldRotManager";
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
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
        }
    }

    public float GetRotationLeft()
    {
        return totalDegrees;
    }
}
