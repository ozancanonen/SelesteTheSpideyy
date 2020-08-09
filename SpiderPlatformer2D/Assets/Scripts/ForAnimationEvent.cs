using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnimationEvent : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    //THİS İS FOR PLAYER. Player has grapple as a child.That child can't access PlayerController. That is why i did this class.
    
    public void AttackEvent() //calling from Animation Event
    {
        playerController.AttackEvent();
    }
}
