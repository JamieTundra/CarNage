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
    float endTime;
    float timeToSixty;
    float chosenMass;
    float chosenToque;
    float maxMass;
    float minMass;
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
        carController.maxWheelRPM = 1500;
        chosenMass = carController.rigidBody.mass;
        chosenToque = carController.maxTorque;
        car.transform.position = GameObject.FindGameObjectWithTag("TimeTrialStart").transform.position;
        carController.rigidBody.mass = chosenMass;
    }

    public void RandomValues()
    {
        carController.mass = Mathf.Round(UnityEngine.Random.Range(100, 1500));
        carController.maxTorque = Mathf.Round(UnityEngine.Random.Range(300, 1500));
    }

    private void Update()
    {
        if (!trialComplete)
        {
            carController.rigidBody.isKinematic = false;
            foreach (WheelCollider wheel in carController.wheelColliders)
            {
                wheel.enabled = true;
            }
            if (!timerStarted)
            {
                timerStarted = true;
                startTime = Time.time;
                //Debug.Log("Time Trial Started");
            }

            if (timerStarted && carController.currentSpeed < 60)
            {
                carController.Drive(false, 1);
            }
            else if (timerStarted && carController.currentSpeed >= 60)
            {
                endTime = Time.time;
                timeToSixty = endTime - startTime;

                if (TimeTrialValues.instance.ZeroToSixtyValues.Count == 1)
                {
                    if (TimeTrialValues.instance.ZeroToSixtyValues[0].timeTaken > timeToSixty)
                    {
                        TimeTrialValues.instance.ZeroToSixtyValues.RemoveAt(0);
                        ZeroToSixty v = new ZeroToSixty
                        {
                            mass = chosenMass,
                            timeTaken = Mathf.Round(timeToSixty * 100f) / 100f,
                            torque = chosenToque,
                        };

                        TimeTrialValues.instance.AddValue(v);
                        trialComplete = true;
                    }
                }
                else
                {
                    ZeroToSixty v = new ZeroToSixty
                    {
                        mass = chosenMass,
                        timeTaken = Mathf.Round(timeToSixty * 100f) / 100f,
                        torque = chosenToque,
                    };

                    TimeTrialValues.instance.AddValue(v);
                    trialComplete = true;
                }
            }
        }

    }

    public void StartTrial()
    {
        trialComplete = false;
    }

    public void ResetTrial()
    {
        carController.rigidBody.isKinematic = true;
        foreach (WheelCollider wheel in carController.wheelColliders)
        {
            wheel.enabled = false;
        }
        timerStarted = false;
        chosenMass = carController.rigidBody.mass;
        chosenToque = carController.maxTorque;
        carController.rigidBody.mass = chosenMass;
        car.transform.position = GameObject.FindGameObjectWithTag("TimeTrialStart").transform.position;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = Color.black;
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;

        if (TimeTrialValues.instance.ZeroToSixtyValues.Count > 0)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Time Taken: " + TimeTrialValues.instance.ZeroToSixtyValues[0].timeTaken + " seconds", style);
            GUI.Label(new Rect(10, 30, 100, 20), "Mass used: " + TimeTrialValues.instance.ZeroToSixtyValues[0].mass, style);
            GUI.Label(new Rect(10, 50, 100, 20), "Torque used: " + TimeTrialValues.instance.ZeroToSixtyValues[0].torque, style);
        }

        GUI.Label(new Rect(10, 70, 100, 20), "Current Mass: " + carController.mass, style);
        GUI.Label(new Rect(10, 90, 100, 20), "Current Torque: " + carController.maxTorque, style);
    }
}

