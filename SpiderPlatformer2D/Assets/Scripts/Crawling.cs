using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawling : MonoBehaviour
{

    public float moveSpeed;
    public GameObject[] wayPoints;
    [SerializeField] float crawlingEnemyDamage = 10f;
    int nextWaypoint = 1;
    bool isDead = false;
    float distToPoint;          //This will store the remaining distance between player and NextWaypoint
    [SerializeField] Animator animator;
    [SerializeField] GameObject hitParticle;
    [SerializeField] Transform dieParticleSpawnPoint;
    // Update is called once per frame
    void Update()
    {
        if(isDead) { return; }
        Move();
    }

    void Move()
    {
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextWaypoint].transform.position);

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextWaypoint].transform.position,
                                    moveSpeed * Time.deltaTime);

        if (distToPoint < 0.1f)
        {
            TakeTurn();
        }
    }

    void TakeTurn()
    {
        Vector3 currRot = transform.eulerAngles;
        currRot.z += wayPoints[nextWaypoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextWaypoint();
    }

    void ChooseNextWaypoint()
    {
        nextWaypoint++;

        if (nextWaypoint == wayPoints.Length)
        {
            nextWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDead) { return; }
        if(collision.gameObject.CompareTag("WebBullet"))
        {
            Die();
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().UpdateHealth(crawlingEnemyDamage);
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("getSquashed");
        var particle = Instantiate(hitParticle, dieParticleSpawnPoint.position, Quaternion.identity);
        Destroy(particle, 1f);
        Destroy(gameObject, 0.28f);
    }
}