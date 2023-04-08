using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelIndicatorBehavior : MonoBehaviour
{
    private List<GameObject> myChildren = new List<GameObject>();
    public int numOfLevels = 0;
    public int curLevel = 0;
    public Color levelColor;
    public GameObject curLevelIndicatorPrefab;
    private void Start()
    {
        int nums = transform.childCount;
        for (int i = 0; i < nums; i++)
        {
            myChildren.Add(transform.GetChild(i).gameObject);
        }
        for (int i = numOfLevels; i < nums; i++)
        {
            myChildren[i].SetActive(false);
        }
        for (int i = 0; i <= curLevel; i++)
        {
            myChildren[i].GetComponent<SpriteRenderer>().color = levelColor;
        }
        Vector3 spawnPos = myChildren[curLevel].transform.position;
        GameObject indicator = Instantiate(curLevelIndicatorPrefab, spawnPos, Quaternion.identity);
        indicator.transform.parent = transform;
    }
}
