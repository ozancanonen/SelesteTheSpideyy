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
        Invoke("QuitTheGame", time+5);
    }

    private void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void QuitTheGame()
    {
        Application.Quit();
    }
}
