using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPadScript : MonoBehaviour
{
    public float launchForce = 1000.0f;
    public GameObject particlePrefab;
    [SerializeField] private AudioClip[] myClips;
    private AudioSource mySource;
    private Color orgColor;
    private void Start()
    {
        orgColor = GetComponent<SpriteRenderer>().color;
        mySource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();
        Vector2 myUp = transform.up;
        if (otherRB != null)
        {
            StartCoroutine(CollideBehavior());

            GameObject particle = Instantiate(particlePrefab, other.contacts[0].point, Quaternion.identity);
            particle.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            particle.GetComponent<ParticleSystem>().Play();
            otherRB.AddForce(myUp * launchForce);
            mySource.clip = myClips[Random.Range(0, myClips.Length)];
            mySource.pitch = Random.Range(0.5f, 0.75f);
            mySource.Play();
        }
    }

    IEnumerator CollideBehavior()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<SpriteRenderer>().color = orgColor;
    }
}
