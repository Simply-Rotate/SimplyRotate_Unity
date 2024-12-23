using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DeliveryBehavior : MonoBehaviour
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject colParticles;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private List<AudioClip> collision;
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
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            gController.FinishLevel(false);
        }
    }

    private void Update()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down);
        
        if (hitDown)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Enemy") && !gController.GetLevelFinished())
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
                // Debug.Log(myRb.velocity.sqrMagnitude);
                mySource.volume = Mathf.Clamp01(myRb.velocity.sqrMagnitude);
                mySource.PlayOneShot(collision[Random.Range(0, collision.Count)]);
            }
        }

        if (canComeBack && other.gameObject.CompareTag("Level"))
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
            mySource.pitch = Random.Range(0.7f, 1.0f);
            mySource.volume = 0.7f;
            mySource.PlayOneShot(powerUp);
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
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(TimeOut());
        }
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
