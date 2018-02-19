using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Car parts
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public Transform centerOfMass;
    public Rigidbody rigidBody;

    // Car variables
    public Car carData;
    float mass;
    float maxSpeed;
    float maxTorque;
    float turnForce;
    float maxBrakeTorque;
    float currentSpeed;

    // Debugging
    [HideInInspector]
    public GUIStyle style;
    public bool debugMode;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        rigidBody = GetComponent<Rigidbody>();
        mass = carData.m_mass;
        maxSpeed = carData.m_maxSpeed;
        maxTorque = carData.m_maxTorque;
        turnForce = carData.m_turnForce;
        maxBrakeTorque = carData.m_maxBrakeTorque;
    }

    public void Steer(float steer)
    {
        float finalAngle = steer * turnForce;
        wheelColliders[0].steerAngle = finalAngle;
        wheelColliders[1].steerAngle = finalAngle;
    }

    public void Drive(bool handBrake, float brake, float accelerate)
    {
        currentSpeed = 2.23694f * rigidBody.velocity.magnitude;

        if (currentSpeed < maxSpeed && !handBrake)
        {
            if (brake == 0)
            {  
                wheelColliders[0].motorTorque = maxTorque * accelerate;
                wheelColliders[1].motorTorque = maxTorque * accelerate;
            }
            else
            {
                wheelColliders[0].motorTorque = maxTorque * brake;
                wheelColliders[1].motorTorque = maxTorque * brake;
            }
        }
        else
        {
            wheelColliders[0].motorTorque = 0;
            wheelColliders[1].motorTorque = 0;
        }
    }

    public void Brake(bool handBrake)
    {
        if (handBrake)
        {
            wheelColliders[2].brakeTorque = maxBrakeTorque;
            wheelColliders[3].brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelColliders[2].brakeTorque = 0;
            wheelColliders[3].brakeTorque = 0;
        }
    }

    void OnGUI()
    {
        if (debugMode)
        {
            style.normal.textColor = Color.white;
            style.fontSize = 16;

            // Debugging labels
            GUI.Label(new Rect(10, 10, 1500, 50), "Max Torque: " + maxTorque, style);
            GUI.Label(new Rect(10, 30, 1500, 50), "Turn Force: " + turnForce, style);
            GUI.Label(new Rect(10, 50, 1500, 50), "Mass: " + mass, style);
            GUI.Label(new Rect(10, 70, 1500, 50), "Current Speed: " + Mathf.Round(currentSpeed) + " MPH", style);
            //GUI.Label(new Rect(10, 70, 1500, 50), "Health: " + health, style);
            //GUI.Label(new Rect(10, 90, 1500, 50), "Damage: " + damage, style);
            //GUI.Label(new Rect(10, 110, 1500, 50), "Immune Status: " + isImmune, style);
            //GUI.Label(new Rect(10, 130, 1500, 50), "Immune Timer: " + immuneTimer, style);
            //GUI.Label(new Rect(10, 150, 1500, 50), "Handbrake Applied: " + handBrake, style);


            //GUI.Label(new Rect(10, 130, 500, 50), "Health: " + health, style);
        }
    }
}
