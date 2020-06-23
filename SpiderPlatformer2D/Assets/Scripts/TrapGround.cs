﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGround : MonoBehaviour
{
    [SerializeField] GameObject trapGroundsHere;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            trapGroundsHere.SetActive(true);
        }

    }
}
