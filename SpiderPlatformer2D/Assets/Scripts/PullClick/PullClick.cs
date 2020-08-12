using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpringJoint2D))]
public abstract class PullClick : MonoBehaviour
{

    public bool isPulling;
    [SerializeField] SpringJoint2D springJoint;
    [SerializeField] float forceMultiplier = 100f;
    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
            Vector3 direction = target.transform.position - transform.position;
            rigidbody.AddForce(direction * forceMultiplier);
        }
    }

    private void DisableSprintJoint()
    {
        springJoint.connectedBody = null;
        springJoint.enabled = false;
        isPulling = false;
    }
}
