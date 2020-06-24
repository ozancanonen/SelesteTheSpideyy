using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gotomainmenu : MonoBehaviour
{
    [SerializeField]float time;
    private void Start()
    {
        Invoke("goToMainMenu", time);
    }

private void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
