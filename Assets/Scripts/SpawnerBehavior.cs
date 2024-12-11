using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    public GameObject deliveryPrefab;
    private bool hasSpawned = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindObjectsOfType<DeliveryBehavior>().Length == 0)
        {
            if (!hasSpawned)
            {
                GameObject delivery = Instantiate(deliveryPrefab, transform.position, Quaternion.identity);
                //delivery.transform.SetParent(FindObjectOfType<ControlScript>().gameObject.transform);
                hasSpawned = true;
            }
        }
        else
        {
            hasSpawned = false;
        }
    }
}
