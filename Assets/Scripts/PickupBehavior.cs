using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public float addAngleAmount = 50.0f;
    [SerializeField] private GameObject particle;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<RotationManager>().AddRotation(addAngleAmount);
            GameObject particles = Instantiate(particle, other.transform.position, Quaternion.identity);
            particles.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            particles.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }
}
