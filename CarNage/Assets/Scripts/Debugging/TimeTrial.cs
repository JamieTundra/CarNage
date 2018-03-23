using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTrial : MonoBehaviour
{
    public static TimeTrial instance;
    bool timerStarted = false;
    bool trialComplete = true;
    float startTime;
    float twentyTime;
    float sixtyTime;
    float hundredTime;
    float timeToSixty;
    float chosenMass;
    float chosenToque;
    float maxMass;
    float minMass;
    bool twentyDone = false;
    bool sixtyDone = false;
    Transform timeTrialStart;
    GameObject car;
    CarController carController;
    public GameObject TimeTrialValuesHolder;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        InitReferences();
        InitTimeTrial();
    }

    private void InitReferences()
    {
        if (GameObject.FindGameObjectWithTag("TimeTrialValues") == null)
        {
            GameObject go = Instantiate(TimeTrialValuesHolder);
        }

        car = GameObject.FindGameObjectWithTag("Player1");
        carController = car.GetComponent<CarController>();
    }

    void InitTimeTrial()
    {
        car.GetComponent<InputHandler>().enabled = false;
        chosenMass = carController.rigidBody.mass;
        chosenToque = carController.torque;
        car.transform.position = GameObject.FindGameObjectWithTag("TimeTrialStart").transform.position;
    }

    private void Update()
    {
        if (!trialComplete)
        {
            if (!timerStarted)
            {
                timerStarted = true;
                startTime = Time.time;
                //Debug.Log("Time Trial Started");
            }

            if (timerStarted && carController.mphSpeed < 100)
            {
                carController.Drive(1);
            }

            if (timerStarted && carController.mphSpeed >= 20 && !twentyDone)
            {
                twentyTime = Time.time - startTime;
                twentyDone = true;
            }
            else if (timerStarted && carController.mphSpeed >= 60 && !sixtyDone)
            {
                sixtyTime = Time.time - startTime;
                sixtyDone = true;
            }
            else if (timerStarted && carController.mphSpeed >= 100)
            {
                hundredTime = Time.time - startTime;
                TrialTime v = new TrialTime
                {
                    timeTakenToTwenty = twentyTime,
                    timeTakenToSixty = sixtyTime,
                    timeTakenToHundred = hundredTime,
                    mass = chosenMass,
                    torque = chosenToque,
                };
                TimeTrialValues.instance.AddValue(v);
                Debug.Log("20: " + v.timeTakenToTwenty + " | 60: " + v.timeTakenToSixty + " | 100: " + v.timeTakenToHundred + " | Mass: " + v.mass + " | Torque: " + v.torque);
                ResetTrial();
            }
        }
        else
        {
            ResetTrial();
        }

    }

    public void StartTrial()
    {
        trialComplete = false;
        carController.rigidBody.isKinematic = false;
    }

    public void ResetTrial()
    {
        trialComplete = true;
        foreach (WheelCollider wheel in carController.wheelColliders)
        {
            wheel.motorTorque = 0;
        }
        car.transform.rotation = Quaternion.Euler(0, 0, 0);
        carController.rigidBody.isKinematic = true;
        timerStarted = false;
        chosenMass = carController.rigidBody.mass;
        chosenToque = carController.torque;
        carController.rigidBody.mass = chosenMass;
        car.transform.position = GameObject.FindGameObjectWithTag("TimeTrialStart").transform.position;
        twentyDone = false;
        sixtyDone = false;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = Color.black;
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;

        GUIStyle boldBigger = new GUIStyle();
        boldBigger.normal.textColor = Color.black;
        boldBigger.fontSize = 25;
        boldBigger.fontStyle = FontStyle.Bold;

        GUI.Label(new Rect(10, 10, 100, 20), "Mass: " + carController.mass, style);
        GUI.Label(new Rect(10, 30, 100, 20), "Torque: " + carController.torque, style);
        GUI.Label(new Rect(10, 50, 100, 20), "CritSpeed: " + carController.criticalSpeed, style);
        GUI.Label(new Rect(10, 70, 100, 20), "Belowstep: " + carController.stepsBelow, style);
        GUI.Label(new Rect(10, 90, 100, 20), "Abovestep: " + carController.stepsAbove, style);
        GUI.Label(new Rect(250, 110, 100, 20), string.Format("Speed: {0:0.00} mph", carController.mphSpeed), boldBigger);
    }
}

