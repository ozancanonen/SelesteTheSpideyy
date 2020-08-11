using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpringJoint2D))]
public abstract class PullClick : MonoBehaviour
{

    public bool isPulling;
    [SerializeField] SpringJoint2D springJoint;

    private void Awake()
    {
        springJoint.enabled = false;
        GameManager.checkSprintJoint += EnableSprintJoint;
        GameManager.deActiveSprintJoint += DisableSprintJoint;
    }
    private void EnableSprintJoint(GameObject target)
    {
        if(isPulling)
        {
            springJoint.enabled = true;
            springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        }
       
    }

    private void DisableSprintJoint()
    {
        springJoint.connectedBody = null;
        springJoint.enabled = false;
        isPulling = false;
    }
}
