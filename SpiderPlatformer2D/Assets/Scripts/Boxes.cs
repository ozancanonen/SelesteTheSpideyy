﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    private void Awake()
    {
        PlayerController.DestroyBoxesInPlayerController += DestroyMe;
        Grapple.DestroyBoxes += DestroyMe;
    }
    public void DestroyMe()
    {
        if (transform.childCount > 0)
        {
            Invoke("DestroyProcess",10f);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void DestroyProcess()
    {
        GameManager.Instance.DeActiveSprintJoint();
        transform.GetChild(0).transform.parent = RopeBridgeController.Instance.transform;
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        RopeBridge ropeBridge = /*FindObjectOfType<RopeBridge>();*/ RopeBridgeController.Instance.GetActiveRope();
        if (ropeBridge != null)
            ropeBridge.SetLastPos(this.transform);
        ropeBridge.transform.parent = this.transform;
    }
    private void OnDisable()
    {
        PlayerController.DestroyBoxesInPlayerController -= DestroyMe;
        Grapple.DestroyBoxes -= DestroyMe;
        RopeBridge ropeBridge = FindObjectOfType<RopeBridge>();
        if (ropeBridge != null)
        {
            ropeBridge.shouldFollow = false;
            ropeBridge.SetTargetToNull();
        }
    }
}
