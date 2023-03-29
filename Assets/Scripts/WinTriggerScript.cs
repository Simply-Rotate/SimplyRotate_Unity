using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTriggerScript : MonoBehaviour
{
    [SerializeField] GameObject winParticles;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Win");
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            if (controller != null)
            {
                controller.GetComponent<GameLogic>().FinishLevel(true);
            }
            GameObject particles = Instantiate(winParticles, other.transform.position, Quaternion.identity);
            particles.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            particles.GetComponent<ParticleSystem>().Play();
        }
    }
}
