using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private AudioSource mySource;
    private Rigidbody2D myRb;
    private void Start()
    {
        mySource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        mySource.pitch = Random.Range(0.7f, 0.8f);
        mySource.volume = myRb.velocity.sqrMagnitude / 80f;
        mySource.Play();
    }
}
