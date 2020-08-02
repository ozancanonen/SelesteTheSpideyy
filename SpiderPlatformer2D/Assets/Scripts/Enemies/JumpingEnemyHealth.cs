using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyHealth : MonoBehaviour,IDamagable
{
    [SerializeField] float damage = 10f;
    [SerializeField] float forceMultiplier = 1000f;
    [SerializeField] Animator animator;
    bool isDead = false;
    


    public void GetDamage(float damageAmount, Transform direction)
    {
        isDead = true;
        animator.SetTrigger("Die");
        Vector3 direct =transform.position -direction.position;
        GetComponent<Rigidbody2D>().AddForce(direct*forceMultiplier);
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) { return; }
        if (collision.gameObject.GetComponent<Thorn>())
        {
            isDead = true;
            animator.SetTrigger("Die");
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1.5f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().UpdateHealth(damage);
        }
    }

   
}
