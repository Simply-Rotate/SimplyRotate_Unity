using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBehavior : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject colParticles;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip collision;
    private Rigidbody2D myRb;
    private AudioSource mySource;
    private bool canPlaySound = false;
    private GameLogic gController;
    public bool canComeBack = false;
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Enemy" && !gController.GetLevelFinished())
        {
            //Time.timeScale = 0.3f;
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
                mySource.clip = collision;
                mySource.pitch = Random.Range(0.75f, 1.0f);
                mySource.volume = myRb.velocity.sqrMagnitude / 80f;
                mySource.Play();
            }
        }

        if ((canComeBack) && other.gameObject.tag == "Level")
        {
            if (other.gameObject.GetComponent<ControlScript>().isInside)
                transform.SetParent(other.transform, true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.gameObject.tag == "Respawner")
        {
            this.transform.position = new Vector3(0, 12, 0);
        }
        else */
        
        if (other.gameObject.tag == "Powerup")
        {
            mySource.clip = powerUp;
            mySource.pitch = Random.Range(0.4f, 0.7f);
            mySource.volume = 0.7f;
            mySource.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Level" && canComeBack)
        {
            other.gameObject.GetComponent<ControlScript>().isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Level")
        {
            canComeBack = false;
            StartTimeOut();
            other.gameObject.GetComponent<ControlScript>().isInside = false;
        }
    }

    public void StartTimeOut()
    {
        StartCoroutine(TimeOut());
    }

    IEnumerator InitPhase()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        canPlaySound = true;
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        canComeBack = true;
    }
}
