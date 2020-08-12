using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBridgeController : MonoBehaviour
{
    private static RopeBridgeController _instance;
    [SerializeField] int ropeNumber = 5;
    [SerializeField] RopeBridge ropePrefab;
    public Queue<RopeBridge> ropeBridges;
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
        ropeBridges = new Queue<RopeBridge>();
        InstantiateRopeBridges();

    }

    private void InstantiateRopeBridges()
    {
        for (int i = 0; i < ropeNumber; i++)
        {
            RopeBridge rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);
            rope.transform.parent = transform;
            rope.gameObject.SetActive(false);
            ropeBridges.Enqueue(rope);
        }
    }

    public RopeBridge GetRopeBridge()
    {
        return null;
    }
}
