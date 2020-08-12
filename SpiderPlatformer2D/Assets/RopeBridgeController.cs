using System;
using System.Collections;
using System.Collections.Generic;
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
        //foreach (RopeBridge rope in ropeBridges)
        //{
        //    if (rope.gameObject.activeInHierarchy &&!rope.hasHolded)
        //    {
        //        rope.gameObject.SetActive(true);
        //        rope.StartPoint.transform.position = grapple.transform.position;
        //        return rope;
        //    }
        //    else
        //    {
        //        spawnCount++;
        //        if(spawnCount >= ropeNumber)
        //        {
        //            spawnCount = 0;
        //        }
        //        var newRope = ropeBridges[spawnCount];
        //        newRope.gameObject.SetActive(true);
        //        CheckSpawnCount(spawnCount);
        //        return newRope;
        //    }
        //}
        //Debug.LogError("Daha fazla rope yok");
        //return null;

        foreach (RopeBridge rope in ropeBridges)
        {
            //if (rope.gameObject.activeInHierarchy&&!rope.shouldFollow &&rope==null) { continue; }
            //rope.gameObject.SetActive(true);
            //rope.shouldFollow = true;
            //rope.StartPoint = grapple.transform;
            //return rope;
            if(rope.name == "Rope 0")
            {
                rope.gameObject.SetActive(true);
                rope.shouldFollow = true;
                rope.StartPoint = grapple.transform;
                return rope;
            }
        }
        Debug.LogError("ROPEBRIDGECONTROLLER NULL DONDURDU");
        return null;
    }
    public RopeBridge GetActiveRope()
    {
        foreach (RopeBridge rope in ropeBridges)
        {
            if (rope.name == "Rope 0")
            {
                rope.gameObject.SetActive(true);
                rope.shouldFollow = true;
                rope.StartPoint = grapple.transform;
                return rope;
            }
        }
        Debug.LogError("ROPEBRIDGECONTROLLER NULL DONDURDU");
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
