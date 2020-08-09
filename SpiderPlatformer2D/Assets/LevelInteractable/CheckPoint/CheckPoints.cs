
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] int currentCameraIndex;
    [SerializeField] Animator plantAnimator;
    [SerializeField] GameObject foregroundParallaxObjects;
    private bool ifCame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerPrefsController.cameraChanged = true;
            PlayerPrefsController.Instance.ChangeCameraIndex(currentCameraIndex);
            PlayerPrefsController.Instance.SavePosition(transform.position.x, transform.position.y);
            if(!ifCame)
            {
                ifCame = true;
                collision.gameObject.GetComponent<PlayerController>().GetHealed();
            }
            plantAnimator.SetBool("Open", true);

            if(foregroundParallaxObjects!=null&&(currentCameraIndex == 1|| currentCameraIndex == 2|| currentCameraIndex == 3))
            {
                foregroundParallaxObjects.SetActive(false);

            }

        }
    }
}
