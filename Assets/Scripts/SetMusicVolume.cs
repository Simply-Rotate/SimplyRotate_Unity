using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMusicVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Slider>().value = GameObject.FindGameObjectWithTag("MusicController").GetComponent<AudioSource>().volume;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectWithTag("MusicController").GetComponent<AudioSource>().volume = gameObject.GetComponent<Slider>().value;
    }
}
