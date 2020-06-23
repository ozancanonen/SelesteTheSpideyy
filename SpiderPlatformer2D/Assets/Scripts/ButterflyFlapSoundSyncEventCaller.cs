using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyFlapSoundSyncEventCaller : MonoBehaviour
{
    public AudioSource flapSound;
public void playFlapWingSound()
    {
        flapSound.Play();
    }
}
