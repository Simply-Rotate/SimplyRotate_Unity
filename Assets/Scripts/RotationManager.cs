using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public float totalRotation = 200.0f;
    public float curRotation = 200.0f;
    public int startLevelIndex = -1;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        List<GameObject> rotMangers = new List<GameObject>(GameObject.FindGameObjectsWithTag("OldRotManager"));
        foreach (GameObject rot in rotMangers)
        {
            Destroy(rot);
        }
    }
}
