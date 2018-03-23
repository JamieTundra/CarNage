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

    #region TrialTime
    [SerializeField]
    public List<TrialTime> trialTime = new List<TrialTime>();

    public void AddValue(TrialTime v)
    {
        trialTime.Add(v); // add new player
    }
    #endregion
}

[System.Serializable]
public class TrialTime
{
    public float mass;
    public float timeTakenToSixty;
    public float timeTakenToTwenty;
    public float timeTakenToHundred;
    public float torque;
}