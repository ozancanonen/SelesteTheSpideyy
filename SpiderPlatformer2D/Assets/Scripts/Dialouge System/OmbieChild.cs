using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmbieChild : MonoBehaviour
{
    [SerializeField] BoxCollider2D interactCol;
     Rigidbody2D rb;
    [SerializeField] Animator wormlingAnim;
    bool isTouched = false;
    bool isHided;

    private void Start()
    {
        Ombie.childCount = 0;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHided&& collision.gameObject.tag == "Player")
        {
            wormlingAnim.SetTrigger("Hide");
            isHided = true;
        }
            if (isTouched) { return; }
        if (collision.gameObject.tag == "InterObject")
        {
            isTouched = true;
            Ombie.childCount++;
            wormlingAnim.SetTrigger("Idle");

            interactCol.enabled = false;
            StartCoroutine(StopRigidBodyAfter(3f));
            if (Ombie.childCount == 2)
            {
                FindObjectOfType<Ombie>().QuestCompleted();
            }
        }
    }
    IEnumerator StopRigidBodyAfter(float time)
    {
        //rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.gravityScale = 5;
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Grapple>().ReleaseGrapple();
        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;
        //GetComponentInChildren<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }
}
