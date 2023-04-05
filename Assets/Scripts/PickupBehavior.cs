using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public float addAngleAmount = 50.0f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<RotationManager>().AddRotation(addAngleAmount);
            Destroy(gameObject);
        }
    }
}
