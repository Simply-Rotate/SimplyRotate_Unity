using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    private static AudioScript instance = null;
    public static AudioScript Instance { get { return Instance; } }
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
        gameObject.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(this);
    }
}
