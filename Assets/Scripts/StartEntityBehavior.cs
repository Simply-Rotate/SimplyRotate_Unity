using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEntityBehavior : MonoBehaviour
{
    public int iconType = 0;
    public GameObject menuPanel;
    public int loadLevelIndex = 2;

    private void Start()
    {
        menuPanel.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && iconType == 0)
        {
            FindObjectOfType<LevelLoader>().LoadThisLevel(loadLevelIndex);
        }
        else if (other.gameObject.tag == "Player" && iconType == 1)
        {
            menuPanel.SetActive(true);
        }
    }
}
