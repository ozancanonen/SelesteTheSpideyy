using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ombie : MonoBehaviour, INPC
{
    private void Start()
    {
        if (ifThrowedWeb)
        {
            GetComponent<Animator>().SetTrigger("HappyIdle");
        }
    }
    public int playersCame { get; set; } = 0;
    bool playerCompleted = false;
    public static bool npcEnder;
    public static int childCount = 0;
    public int cameraIndex = 1;
    public static bool ifThrowedGrapple;
    public static bool ifThrowedWeb;
    [SerializeField] GameObject webObtainObject;
    [SerializeField] GameObject GrappleObtainObject;
    [SerializeField] Vector3 webObtainObjectForce;
    [SerializeField] Transform webObtainObjectSpawnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(npcEnder == true) { return; }
        playersCame++;
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(cameraChangeAfter());
            GetComponent<DialogueTrigger>().TriggerDialogue(playerCompleted, playersCame);
            if(!ifThrowedGrapple)
            StartCoroutine(giveGrappleSkill());
        }
    }

    IEnumerator cameraChangeAfter()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefsController.Instance.ChangeCameraIndex(cameraIndex);
        //oldVirtualCamera.SetActive(false);
        //newVirtualCamera.SetActive(true);
    }
    IEnumerator giveGrappleSkill()
    {
        ifThrowedGrapple = true;
        yield return new WaitForSeconds(3f);
        var webObtain = Instantiate(GrappleObtainObject, webObtainObjectSpawnPos.position, Quaternion.identity);
        webObtain.GetComponent<Rigidbody2D>().AddForce(webObtainObjectForce);
    }
    public void QuestCompleted()
    {
        playerCompleted = true;
        GetComponent<Animator>().SetTrigger("HappyIdle");
        if (!ifThrowedWeb)
        {
            ifThrowedWeb = true;
            var webObtain = Instantiate(webObtainObject, webObtainObjectSpawnPos.position, Quaternion.identity);
            webObtain.GetComponent<Rigidbody2D>().AddForce(webObtainObjectForce);
        }
    }
    IEnumerator giveWebSkill()
    {
        yield return new WaitForSeconds(3f);
        var webObtain = Instantiate(webObtainObject, webObtainObjectSpawnPos.position, Quaternion.identity);
        webObtain.GetComponent<Rigidbody2D>().AddForce(webObtainObjectForce);
    }

}
