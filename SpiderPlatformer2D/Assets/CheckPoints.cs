
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] int currentCameraIndex;
    [SerializeField] Animator plantAnimator;
    [SerializeField] GameObject foregroundParallaxObjects;
    private bool ifCame;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&& !ifCame)
        {
            ifCame = true;
            PlayerPrefsController.cameraChanged = true;
            PlayerPrefsController.Instance.ChangeCameraIndex(currentCameraIndex);
            //PlayerPrefsController.Instance.ChangeCameraState();
            PlayerPrefsController.Instance.SavePosition(transform.position.x, transform.position.y);
            var wormlingHealth = FindObjectsOfType<WormlingHealth>();
            if(wormlingHealth!=null)
            {
                foreach(WormlingHealth wormling in wormlingHealth)
                {
                    wormling.RestoreHealth();
                }
            }
            collision.gameObject.GetComponent<PlayerController>().GetHealed();
            plantAnimator.SetBool("Open", true);

            if(foregroundParallaxObjects!=null&&(currentCameraIndex == 1|| currentCameraIndex == 2|| currentCameraIndex == 3))
            {
                foregroundParallaxObjects.SetActive(false);

            }

        }
    }
}
