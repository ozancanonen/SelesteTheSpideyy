using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    RopeBridge ropeBridge;
    private void Awake()
    {
        PlayerController.DestroyBoxesInPlayerController += DestroyMe;
        Grapple.DestroyBoxes += DestroyMe;
    }
    public void DestroyMe()
    {
       
        if (transform.childCount > 0)
        {
            ropeBridge =transform.GetChild(0).GetComponent<RopeBridge>();
            ropeBridge.transform.parent = RopeBridgeController.Instance.transform;
            ropeBridge.canReturn = false;
            //transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;
            Grapple.isPulling = false;
            Invoke("DestroyProcess",5f);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void DestroyProcess()
    {
        GameManager.Instance.DeActiveSprintJoint();
        //transform.GetChild(0).GetComponent<RopeBridge>().canReturn = true; artık child ' ı değil ki
        ropeBridge.canReturn = true;
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
