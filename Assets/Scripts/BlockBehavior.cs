using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
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
        mySource.pitch = Random.Range(0.75f, 1.0f);
        if (collision.gameObject.tag != "Level")
        {
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                mySource.volume = (myRb.velocity.sqrMagnitude + otherRb.velocity.sqrMagnitude) / 10f;
            }
        }
        else
        {
            mySource.volume = myRb.velocity.sqrMagnitude;
        }
        
        mySource.PlayOneShot(mySource.clip);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
