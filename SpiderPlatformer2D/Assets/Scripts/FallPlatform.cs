using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public float fallPlatformNumber;
    public GameObject fallPlatformParticle;
    public GameObject tempColliderObject;
    public GameObject needToBeClosedParrallaxObject;
    private GameObject playerObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (fallPlatformNumber == 1&& !Ombie.ifThrowedWeb)
        {
            return;
        }
        else if (collision.tag == "Player")
        {
            playerObject = collision.gameObject;
            StartCoroutine(SetPlayerGrasvityLower());
            tempColliderObject.SetActive(false);
            fallPlatformParticle.SetActive(true);
            if (needToBeClosedParrallaxObject != null)
            {
                needToBeClosedParrallaxObject.SetActive(false);
            }
        }
    }
    IEnumerator SetPlayerGrasvityLower()
    {
        float playerGravityScale = playerObject.GetComponent<Rigidbody2D>().gravityScale;
        playerObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        yield return new WaitForSeconds(1f);
        playerObject.GetComponent<Rigidbody2D>().gravityScale = playerGravityScale;
    }
}
