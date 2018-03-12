using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTrial : MonoBehaviour
{
    bool timerStarted = false;
    bool trialComplete = false;
    float startTime;
    float endTime;
    float timeToSixty;
    float chosenMass;
    float chosenToque;
    float maxMass = 1500;
    float minMass;
    Transform timeTrialStart;
    GameObject car;
    CarController carController;
    public GameObject TimeTrialValuesHolder;

    private void Start()
    {
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
        if (TimeTrialValues.instance.ZeroToSixtyValues.Count > 0)
        {
            maxMass = TimeTrialValues.instance.ZeroToSixtyValues[0].mass;
        }

        // Override car caps
        carController.maxWheelRPM = 1500;
        minMass = maxMass - 100;
        if (minMass <= 50)
        {
            minMass = 50;
        }
        carController.rigidBody.mass = Mathf.Round(UnityEngine.Random.Range(100, maxMass));
        Debug.Log(carController.rigidBody.mass);
        chosenMass = carController.rigidBody.mass;
        chosenToque = carController.maxTorque;
        //Debug.Log("Chosen Mass: " + carController.rigidBody.mass);
        car.transform.position = GameObject.FindGameObjectWithTag("TimeTrialStart").transform.position;
        trialComplete = false;

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

            if (timerStarted && carController.currentSpeed < 60)
            {
                carController.Drive(false, 1);
            }
            else if (timerStarted && carController.currentSpeed >= 60)
            {
                endTime = Time.time;
                timeToSixty = endTime - startTime;
                //Debug.Log("Trial Complete");
                //Debug.Log("0-60 in: " + timeToSixty + " seconds");

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
                    }
                }
                else
                {
                    ZeroToSixty v = new ZeroToSixty
                    {
                        mass = chosenMass,
                        timeTaken = timeToSixty,
                        torque = chosenToque,
                    };

                    TimeTrialValues.instance.AddValue(v);
                }

                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
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
            GUI.Label(new Rect(10, 60, 100, 20), "Mass used: " + TimeTrialValues.instance.ZeroToSixtyValues[0].mass, style);
        }

    }
}


