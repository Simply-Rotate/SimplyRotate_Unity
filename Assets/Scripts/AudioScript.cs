using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    private static AudioScript instance = null;
    public static AudioScript Instance { get { return Instance; } }

    [SerializeField] private AudioClip[] myClips;
    private AudioSource mySource;
    private bool isPlayingTitle = false;
    private bool isPlayingLevel = false;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
        mySource = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2 && !isPlayingTitle)
        {
            isPlayingTitle = true;
            isPlayingLevel = false;
            mySource.clip = myClips[0];
            mySource.Play();
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 2 && !isPlayingLevel)
        {
            isPlayingTitle = false;
            isPlayingLevel = true;
            mySource.clip = myClips[1];
            mySource.Play();
        }
    }

}
