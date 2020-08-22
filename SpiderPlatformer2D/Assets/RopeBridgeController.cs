using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeBridgeController : MonoBehaviour
{
    private static RopeBridgeController _instance;
    [SerializeField] int ropeNumber = 5;
    [SerializeField] GameObject ropePrefab;
    public List<RopeBridge> ropeBridges;
    [SerializeField] Grapple grapple;
    int spawnCount = 0;
    public static RopeBridgeController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no RopeBridgeController in scene");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        InstantiateRopeBridges();

    }

    private void InstantiateRopeBridges()
    {
        for (int i = 0; i < ropeNumber; i++)
        {
            GameObject rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);
            rope.transform.parent = transform;
            rope.gameObject.SetActive(false);
            rope.name = "Rope " + i;
            ropeBridges.Add(rope.GetComponent<RopeBridge>());
        }
    }
    public RopeBridge GetRopeFromPool()
    {
        Debug.Log("Rope Almaya calıstı");
        //foreach (RopeBridge rope in ropeBridges)
        //{
        //if (rope.gameObject.activeInHierarchy&&!rope.shouldFollow &&rope==null) { continue; }
        //rope.gameObject.SetActive(true);
        //rope.shouldFollow = true;
        //rope.StartPoint = grapple.transform;
        //return rope;
        //if(rope.name == "Rope 0")
        //{
        //    rope.gameObject.SetActive(true);
        //    rope.shouldFollow = true;
        //    rope.StartPoint = grapple.transform;
        //    return rope;
        //}
        //}
        if (!ropeBridges[0].StartPoint.gameObject.GetComponent<PullClick>()&&ropeBridges[0].canReturn)
        {
            return ropeBridges[0];
        }
        else if (!ropeBridges[1].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[1].canReturn)
        {
            return ropeBridges[1];
        }
        else if (!ropeBridges[2].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[2].canReturn)
        {
            return ropeBridges[2];
        }
        else if (!ropeBridges[3].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[3].canReturn)
        {
            return ropeBridges[3];
        }
        else if (!ropeBridges[4].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[4].canReturn)
        {
            return ropeBridges[4];
        }
        Debug.LogError("ROPEBRIDGECONTROLLER NULL DONDURDU");
        return null;
    }
    public RopeBridge GetActiveRope()
    {
        if (!ropeBridges[0].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[0].canReturn)
        {
            return ropeBridges[0];
        }
        else if (!ropeBridges[1].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[1].canReturn)
        {
            return ropeBridges[1];
        }
        else if (!ropeBridges[2].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[2].canReturn)
        {
            return ropeBridges[2];
        }
        else if (!ropeBridges[3].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[3].canReturn)
        {
            return ropeBridges[3];
        }
        else if (!ropeBridges[4].StartPoint.gameObject.GetComponent<PullClick>() && ropeBridges[4].canReturn)
        {
            return ropeBridges[4];
        }
        Debug.LogError("ROPEBRIDGECONTROLLER NULL DONDURDU");
        return null;
    }
    public RopeBridge GetPulledRope()
    {
        foreach (RopeBridge rope in ropeBridges)
        {
            if (rope.StartPoint.GetComponent<PullClick>())
            {
                return rope;
            }
        }
        return null;
    }
    private void CheckSpawnCount(int spawnCount)
    {
        if(spawnCount > ropeNumber)
        {
            Debug.Log("You have reached maximum number");
        }
    }
}
