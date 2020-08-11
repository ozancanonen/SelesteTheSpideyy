using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBullet : MonoBehaviour
{
    private Grapple grapple;
    [SerializeField] GameObject grappableObject;
    [SerializeField] GameObject webParticle;
    PlayerController playerController;
    //PullClick Process
    public bool pullClickProcess = false;
    private void OnEnable()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Grappable"|| collision.gameObject.tag == "Pullable")
        {
            Quaternion angle = Quaternion.identity;
            angle.eulerAngles = grapple.shootPoint.eulerAngles + new Vector3(0, 0, -90f);
            ContactPoint2D contact = collision.contacts[0];
            GameObject bulletInstance = Instantiate(grappableObject, contact.point, Quaternion.identity);
            GameObject webPrefab = Instantiate(webParticle, contact.point, angle);
            webPrefab.transform.parent = collision.transform;
            bulletInstance.transform.parent = collision.transform;

            if (playerController.IsPlayerHoldingMouse1() && GameManager.Instance.isPullClick)
            {
                pullClickProcess = true;
                Debug.Log("Going another bullet");
                grapple.ReleaseGrapple();
                GameManager.Instance.target = bulletInstance.gameObject;
                GameManager.Instance.CheckSprintJoint();
                playerController.ropeBridge.StartPoint = GetPullable();
                //foreach(GrappleBullet bullet in FindObjectsOfType<GrappleBullet>())
                //{
                //    if(bullet!=this)
                //    {
                //        grappkl
                //    }
                //}
            }
            else
            {
                GameManager.Instance.isPullClick = false;
                if (collision.gameObject.tag == "Grappable")
                {
                    GameManager.Instance.isPullClick = false;
                    grapple.TargetHit(bulletInstance);
                }
                if (collision.gameObject.tag == "Pullable")
                {
                    PullClick pull = collision.gameObject.GetComponent<PullClick>();
                    if (pull != null)
                    {
                        pull.isPulling = true;
                        GameManager.Instance.isPullClick = true;
                    }
                    else
                    {
                        GameManager.Instance.isPullClick = false;
                    }
                    grapple.PullableHit(collision.gameObject);
                }
            }
            //if(!PullClickProcess)
            //{

            //}
            //else
            //{
            //    Debug.Log("I GUESS IT IS WORKING");
            //}

            Destroy(gameObject);
        }
    }

    private Transform GetPullable()
    {
        foreach (PullClick pullClick in FindObjectsOfType<PullClick>())
        {
            if (pullClick.isPulling == true)
            {
                return pullClick.transform;
            }
        }
        return null;
    }

    public void SetGrapple(Grapple grapple)
    {
        this.grapple = grapple;
    }
    
}
