using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCollidier : MonoBehaviour
{
    public AudioManager audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        audioManager.Play("BossMusic");
    }
}

