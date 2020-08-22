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
    public static bool isGrappled = false;
    public static bool isPulling = false;
    [HideInInspector] public GameObject target;
    //RopeProcess
    Transform lastPos;
    /// <summary>
    /// PULL CLLİCK PROCESS /////////////////////////

    public delegate void DisableRopeBridges();
    public static event DisableRopeBridges DisableRopeBridgesEvent;

    /// </summary>
   
    float timeToGrapple = 0;

    private void Start()
    {
        //lineRenderer.enabled = false;
        springJoint.enabled = false;
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
               
                //ExitGrapple();
                if(DisableRopeBridgesEvent!=null)
                {
                    DisableRopeBridgesEvent();
                }

                if (DestroyWebsInGrapple != null)
                {
                    DestroyWebsInGrapple();
                }
                if (DestroyBoxes != null)
                {
                    DestroyBoxes();
                }


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
        GameManager.Instance.DeActiveSprintJoint();
        timeToGrapple = 0;
        target = null;
        DisableSprintJoint();
        GetComponentInParent<Rigidbody2D>().gravityScale = playerController.gravityDefaultValue;
        isGrappled = false;
        isPulling = false;
        //Rope process
        RopeBridge rope = RopeBridgeController.Instance.GetPulledRope(); //Burayı düzeltmem lazım
        if(rope!=null)
        {
            Debug.Log(rope.name);
        }
        if(rope==null)//ACABAAA ? 
        {
            rope = RopeBridgeController.Instance.GetActiveRope();
        }
        rope.EndPoint.position = this.transform.position; //transform.positin = this.transform.position idi
        rope.gameObject.SetActive(false);
        rope.SetTargetToNull();
        rope.shouldFollow = false;
    }
    IEnumerator ExitGrappleWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        timeToGrapple = 0;
        target = null;
        DisableSprintJoint();
        GetComponentInParent<Rigidbody2D>().gravityScale = playerController.gravityDefaultValue;
        isGrappled = false;
        isPulling = false;
        //Rope process
        RopeBridge rope = RopeBridgeController.Instance.GetActiveRope();
        rope.EndPoint.position = this.transform.position;
        rope.gameObject.SetActive(false);
        rope.SetTargetToNull();
        rope.shouldFollow = false;
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
            Debug.Log("Rope Shoot");
            //RopeBridge ropeBridge =  RopeBridgeController.Instance.GetRopeFromPool();
            GameManager.Instance.isPullClick = false;
            RotateGrapple();
            timeToGrapple = 0;
            GameObject bulletInstance = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletInstance.GetComponent<GrappleBullet>().SetGrapple(this); // this method will called immedialty when bullet instance is born.
            bulletInstance.GetComponent<Rigidbody2D>().AddForce(shootPoint.right * bulletSpeed);
            RopeBridge rope = RopeBridgeController.Instance.GetRopeFromPool(); //getactiverope idi
            Debug.Log(rope.name);
            rope.gameObject.SetActive(true);
            rope.SetTarget(bulletInstance.transform);
            rope.shouldFollow = true;
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
        RopeBridge rope = RopeBridgeController.Instance.GetActiveRope(); //getactiverope idi
        rope.gameObject.SetActive(true);
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
        RopeBridge rope = RopeBridgeController.Instance.GetActiveRope();
        rope.gameObject.GetComponent<LineRenderer>().enabled = false ;
    }
}
