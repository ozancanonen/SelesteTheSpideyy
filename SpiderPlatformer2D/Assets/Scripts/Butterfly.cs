using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour, INPC
{
    public int playersCame { get; set; } = 0;
    bool playerCompleted = false;
    public static bool npcEnder;
    public static int childCount = 0;
    public int cameraIndex = 1;
    [SerializeField] GameObject glideObtainObject;
    [SerializeField] Vector3 glideObtainObjectForce;
    [SerializeField] Transform glideObtainObjectSpawnPos;
    bool ifThrowedGlide;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (npcEnder == true) { return; }
        Debug.Log("player came");
        playersCame++;
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(cameraChangeAfter());
            GetComponent<DialogueTrigger>().TriggerDialogue(playerCompleted, playersCame);

            StartCoroutine(giveGlideSkill());
        }
    }

    IEnumerator cameraChangeAfter()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefsController.Instance.ChangeCameraIndex(cameraIndex);
        //oldVirtualCamera.SetActive(false);
        //newVirtualCamera.SetActive(true);
    }
    public void QuestCompleted()
    {
        playerCompleted = true;
        GetComponent<Animator>().SetTrigger("HappyIdle");
        Web_Projectile.canWeb = true;
    }
    IEnumerator giveGlideSkill()
    {
        ifThrowedGlide = true;
        yield return new WaitForSeconds(3f);
        var webObtain = Instantiate(glideObtainObject, glideObtainObjectSpawnPos.position, Quaternion.identity);
        webObtain.GetComponent<Rigidbody2D>().AddForce(glideObtainObjectForce);
    }

}
