using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PullClick : MonoBehaviour
{

    public bool isPulling;
    [SerializeField] SpringJoint2D springJoint;

    private void Awake()
    {
        springJoint.enabled = false;
    }
    private void EnableSprintJoint(GameObject target)
    {
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
    }

    private void DisableSprintJoint()
    {
        springJoint.enabled = false;
    }
}
