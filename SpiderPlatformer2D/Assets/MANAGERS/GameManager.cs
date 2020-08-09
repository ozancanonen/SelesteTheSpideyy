using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void ShakeCamera();
    public static event ShakeCamera shakeCurrentCam;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("There is no Game Manager in scene.");
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    #endregion

    public void ShakeEvent()
    {
        shakeCurrentCam();
    }

}
