using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBehavior : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject colParticles;
    private Rigidbody2D myRb;
    private AudioSource mySource;
    private bool canPlaySound = false;
    private GameLogic gController;
    private void Start()
    {
        mySource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody2D>();
        gController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLogic>();
        StartCoroutine(InitPhase());
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Enemy" && !gController.GetLevelFinished())
        {
            gController.FinishLevel(false);
            GameObject particles = Instantiate(deathParticles, other.transform.position, Quaternion.identity);
            particles.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            particles.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
        else
        {
            GameObject particles = Instantiate(colParticles, other.contacts[0].point, Quaternion.identity);
            particles.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            var myEmmission = particles.GetComponent<ParticleSystem>().emission;
            myEmmission.rateOverTime = myRb.velocity.sqrMagnitude * 100;
            particles.GetComponent<ParticleSystem>().Play();
            if (canPlaySound)
            {
                mySource.pitch = Random.Range(0.75f, 1.0f);
                mySource.volume = myRb.velocity.sqrMagnitude / 80f;
                mySource.Play();
            }
        }
    }

    IEnumerator InitPhase()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        canPlaySound = true;
    }
}
