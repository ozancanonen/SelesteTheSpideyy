using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void ShakeCamera();
    public static event ShakeCamera shakeCurrentCam;

    public delegate void EnableSprintJoint(GameObject target);
    public static event EnableSprintJoint checkSprintJoint;
    public delegate void DisableSprintJoint();
    public static event DisableSprintJoint deActiveSprintJoint;
    public GameObject target;
    public bool isPullClick;
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
    public void CheckSprintJoint()
    {
        checkSprintJoint(target);
    }

    public void DeActiveSprintJoint()
    {
        deActiveSprintJoint();
    }
}
