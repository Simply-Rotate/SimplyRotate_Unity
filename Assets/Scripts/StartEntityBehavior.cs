using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEntityBehavior : MonoBehaviour
{
    public int iconType = 0;
    public GameObject menuPanel;

    private void Start()
    {
        menuPanel.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && iconType == 0)
        {
            FindObjectOfType<LevelLoader>().LoadThisLevel(2);
        }
        else if (other.gameObject.tag == "Player" && iconType == 1)
        {
            menuPanel.SetActive(true);
        }
    }
}
