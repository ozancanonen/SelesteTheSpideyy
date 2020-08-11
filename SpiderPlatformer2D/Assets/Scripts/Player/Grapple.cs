using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed = 2000f;
    public PlayerController playerController;
    public delegate void DestroySomeStuffInGrapple();
    public static event DestroySomeStuffInGrapple DestroyBoxes;
    public delegate void DestroyWeb();
    public static event DestroyWeb DestroyWebsInGrapple;
    public Transform shootPoint;
    public LineRenderer lineRenderer;
    public SpringJoint2D springJoint;
    public static bool canGrapple = false;
    [HideInInspector] public bool isGrappled = false;
    [HideInInspector] public GameObject target;
    //RopeProcess
    [SerializeField] GameObject rope;
    RopeBridge ropeBridge;
    Transform lastPos;

    bool isPulling = false;
    float timeToGrapple = 0;

    private void Start()
    {
        //lineRenderer.enabled = false;
        springJoint.enabled = false;
        ropeBridge = rope.GetComponent<RopeBridge>();
        rope.SetActive(false);
    }
    private void Update()
    {
        if (playerController.isAlive)
        {
            if(!canGrapple) { return; }
            if (Input.GetMouseButton(1) && !isGrappled && !isPulling)
            {
                Shoot();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                //ropeBridge.StartPoint.position = transform.position;
                //ropeBridge.EndPoint.position = transform.position;
                GameManager.Instance.DeActiveSprintJoint();
                if (DestroyWebsInGrapple != null)
                {
                    DestroyWebsInGrapple();
                }
                if (DestroyBoxes != null)
                {
                    DestroyBoxes();
                }

                ExitGrapple();
            }
            if (target != null)
            {
                //lineRenderer.SetPosition(0, shootPoint.position);
                //lineRenderer.SetPosition(1, target.transform.position);
                //webEndPos.transform.parent = target.transform;
            }
            else
            {
                //webEndPos.SetActive(false);
                //webEndPos.transform.parent = this.transform;
                //lineRenderer.enabled = false;
            }

        }
    }

    public void ExitGrapple()
    {
        timeToGrapple = 0;
        target = null;
        DisableSprintJoint();
        GetComponentInParent<Rigidbody2D>().gravityScale = playerController.gravityDefaultValue;
        isGrappled = false;
        isPulling = false;
        //Rope process
        ropeBridge.EndPoint.position = this.transform.position;
        rope.SetActive(false);
        ropeBridge.SetTargetToNull();
        ropeBridge.shouldFollow = false;
    }

    private void RotateGrapple()
    {
        Vector2 direction = GetMousePos() - (Vector2)transform.position;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }
    private void Shoot()
    {
        timeToGrapple += Time.deltaTime;
        if (timeToGrapple >= 0.3f)
        {
            GameManager.Instance.isPullClick = false;
            RotateGrapple();
            timeToGrapple = 0;
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletInstance.GetComponent<GrappleBullet>().SetGrapple(this); // this method will called immedialty when bullet instance is born.
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
            rope.SetActive(true);
            ropeBridge.SetTarget(bulletInstance.transform);
            ropeBridge.shouldFollow = true;
            AudioManager.Instance.Play("SpiderGrappleShoot");
            Destroy(bulletInstance, 0.6f);
        }
    }
    public Transform ShootForPullClick()
    {
        RotateGrapple();
        timeToGrapple = 0;
        GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletInstance.GetComponent<GrappleBullet>().SetGrapple(this); // this method will called immedialty when bullet instance is born.
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
        rope.SetActive(true);
        AudioManager.Instance.Play("SpiderGrappleShoot");
        Debug.Log("Working");
        Destroy(bulletInstance, 0.6f);
        return bulletInstance.transform;
    }
    //IEnumerator DestroyGrappleAfter(GameObject bullet)
    //{
    //    if(bullet!=null)
    //    {
    //        lastPos = bullet.transform;
    //        ropeBridge.SetLastPos(lastPos);
    //    }
    //    yield return new WaitForSeconds(0.6f);
    //    Destroy(bullet);

    //}
    public void TargetHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        TargetSelection(hit);
        isGrappled = true;
    }
    public void PullableHit(GameObject hit) //when our hidden bullet hits the object with Grappable tag , we will call this method from GrappleBullet
    {
        TargetSelection(hit);
        isPulling = true;
    }
    void TargetSelection(GameObject hit)
    {
        target = hit;
        springJoint.enabled = true;
        springJoint.connectedBody = target.GetComponent<Rigidbody2D>();
        //lineRenderer.enabled = true;
    }
    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public bool GetIsGrapple()
    {
        return isGrappled;
    }
    public bool GetIsPulling()
    {
        return isPulling;
    }
    public GameObject GetTarget()
    {
        return target;
    }
    public Vector3 GetTargetPos()
    {
        return target.transform.position;
    }
    public void DisableSprintJoint()
    {
        springJoint.enabled = false;
    }
    public void ReleaseGrapple()
    {
        DisableSprintJoint();
        target = null;
        isGrappled = false;
    }
    public void DeActiveRope()
    {
        rope.SetActive(false);
    }
}
