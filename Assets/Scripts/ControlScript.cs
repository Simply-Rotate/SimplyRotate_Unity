using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    public float rotateSpeed = 10.0f;
    private float totalDegrees = 0.0f;
    public Slider rotateBar;

    private Rigidbody2D rb;
    private Vector3 myDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetRotateAmount(float curAmount, float maxAmount)
    {
        totalDegrees = curAmount;
        rotateBar.maxValue = maxAmount;
        rotateBar.value = curAmount;
    }

    void FixedUpdate()
    {
        /*float funny = Input.GetAxis("Vertical");*/
        float turn = Input.GetAxis("Horizontal");
        /*transform.Rotate(-funny * rotateSpeed, 0f, -turn * rotateSpeed);*/
        if (totalDegrees >= 0.0f)
        {
            transform.Rotate(0f, 0f, -turn * rotateSpeed);
            totalDegrees -= Mathf.Abs(turn);
        }
        rotateBar.value = totalDegrees;

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
