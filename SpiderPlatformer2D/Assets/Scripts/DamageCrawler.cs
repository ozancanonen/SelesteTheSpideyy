using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCrawler : MonoBehaviour
{
    [SerializeField] Crawling crawling;
    //[SerializeField] PatrolBombs patrolBombs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            //buralarda animasyon olucak lan mk particle marticle de olur.. lan amına koyim
            crawling.Die();
            //patrolBombs.InstantiateBombs();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1200));
        }
    }
}
