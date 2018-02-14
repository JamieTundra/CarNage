using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singelton
    public static InputManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    #endregion

    #region Inputs

    #endregion
}


