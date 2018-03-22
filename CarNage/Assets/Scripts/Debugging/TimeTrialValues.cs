using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialValues : MonoBehaviour
{
    public static TimeTrialValues instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    #region ZeroToSixtyValues
    [SerializeField]
    public List<ZeroToSixty> ZeroToSixtyValues = new List<ZeroToSixty>();

    public void AddValue(ZeroToSixty v)
    {
        ZeroToSixtyValues.Add(v); // add new player
    }
    #endregion
}

[System.Serializable]
public class ZeroToSixty
{
    public float mass;
    public float timeTaken;
    public float torque;
}