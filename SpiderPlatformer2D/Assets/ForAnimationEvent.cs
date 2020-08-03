using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

 
    
    public void AttackEvent() //calling from Animation Event
    {
        playerController.AttackEvent();
    }
}
