using System.Collections;
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
        Destroy(this.gameObject);
    }
    private void OnEnable()
    {
        RopeBridge ropeBridge = FindObjectOfType<RopeBridge>();
        ropeBridge.SetLastPos(this.transform);
    }
    private void OnDisable()
    {
        RopeBridge ropeBridge = FindObjectOfType<RopeBridge>();
        ropeBridge.shouldFollow = false;
        ropeBridge.SetTargetToNull();
        PlayerController.DestroyBoxesInPlayerController -= DestroyMe;
        Grapple.DestroyBoxes -= DestroyMe;
    }
}
