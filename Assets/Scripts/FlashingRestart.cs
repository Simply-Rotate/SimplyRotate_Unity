using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingRestart : MonoBehaviour
{
    public Slider rotateBar;
    public GameObject restartGUI;
    private Image myImg;
    private Color startColor = Color.white;
    private Color endColor = Color.white;
    private float speed = 0.85f;
    private GameObject delivery;
    private void Start()
    {
        endColor.a = 0f;
        myImg = GetComponent<Image>();
        myImg.color = endColor;
        delivery = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        delivery = GameObject.FindGameObjectWithTag("Player");
        if (restartGUI.activeSelf)
        {
            myImg.color = endColor;
        }
        else if (delivery != null)
        {
            if ((rotateBar.value <= 0 && delivery.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f))
            {
                myImg.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
            }
        }
        else
        {
            myImg.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }
    }
}
