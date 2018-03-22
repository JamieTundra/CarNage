using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    // Car parts
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public Transform centerOfMass;
    public Rigidbody rigidBody;


    // Car variables
    public Car carData;
    public float mass;
    public float maxWheelRPM;
    public float maxTorque;
    public float turnForce;
    public float maxBrakeTorque;
    public float currentSpeed;
    public bool isOnGround;
    float averageRPM;
    public bool canSelfRight = true;
    public bool selfRighting = false;
    public bool clearOfGround = false;
    Vector3 currentPosition;
    Vector3 hoverPosition;
    Vector3 currentRotation;
    Vector3 targetRotation;
    float rotationTime;
    float moveTime;

    // Debugging
    [HideInInspector]
    public GUIStyle style;
    public bool debugMode;


    public void Awake()
    {
        InitReferences();
    }

    private void Start()
    {
        InitValues();
    }

    private void InitReferences()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InitValues()
    {

        mass = carData.m_mass;
        maxWheelRPM = carData.m_maxWheelRPM;
        maxTorque = carData.m_maxTorque;
        turnForce = carData.m_turnForce;
        maxBrakeTorque = carData.m_maxBrakeTorque;
        rigidBody.centerOfMass = centerOfMass.transform.position;
        rigidBody.mass = mass;
        this.GetComponent<InputHandler>().m_carInit = true;
    }

    private void Update()
    {
        if (selfRighting)
        {
            moveTime += Time.deltaTime * 2f;

            if (!clearOfGround)
            {
                this.transform.position = Vector3.Lerp(currentPosition, hoverPosition, moveTime);
            }
            else
            {
                rotationTime += Time.deltaTime * 0.5f;
                this.transform.rotation = Quaternion.Slerp(Quaternion.Euler(currentRotation), Quaternion.Euler(0, currentRotation.y, 0), rotationTime);
            }

        }

        if (rotationTime >= 1)
        {
            canSelfRight = true;
            selfRighting = false;
            rigidBody.isKinematic = false;
        }

        if (moveTime >= 1)
        {
            clearOfGround = true;
        }
        Debug.Log(rotationTime);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(this.transform.position, Vector3.up, Color.red);
        Debug.DrawRay(this.transform.position, Vector3.down, Color.red);

        if (Physics.Raycast(this.transform.position, Vector3.up, 0.5f) || (Physics.Raycast(this.transform.position, Vector3.down, 0.5f)))
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }

    public void Steer(float steer)
    {
        float finalAngle = steer * turnForce;
        wheelColliders[0].steerAngle = finalAngle;
        wheelColliders[1].steerAngle = finalAngle;
    }

    public void Drive(bool handBrake, float drivingForce)
    {
        currentSpeed = 2.23694f * rigidBody.velocity.magnitude;

        if (currentSpeed != 0)
        {
            //Debug.Log("Current Speed: " + Mathf.Round(currentSpeed) + " MPH");
        }


        for (int i = 0; i < wheelColliders.Length; i++)
        {
            averageRPM += wheelColliders[i].rpm;
        }

        averageRPM = averageRPM / 4;

        if (averageRPM <= maxWheelRPM)
        {
            wheelColliders[0].motorTorque = drivingForce * maxTorque;
            wheelColliders[1].motorTorque = drivingForce * maxTorque;
        }
        else
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = 0;
            }
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
            wheelColliders[0].brakeTorque = 0;
            wheelColliders[1].brakeTorque = 0;
            wheelColliders[2].brakeTorque = 0;
            wheelColliders[3].brakeTorque = 0;
        }
    }

    public void SelfRight()
    {
        if (canSelfRight && averageRPM < 50 && isOnGround)
        {
            Debug.Log("Attempting to self right");
            selfRighting = true;
            canSelfRight = false;
            clearOfGround = false;
            rigidBody.isKinematic = true;
            currentPosition = this.transform.position;
            hoverPosition = new Vector3(currentPosition.x, currentPosition.y + 0.25f, currentPosition.z);
            currentRotation = this.transform.rotation.eulerAngles;
            targetRotation = new Vector3(0, currentRotation.y, 0);
            rotationTime = 0f;
            moveTime = 0f;
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
            GUI.Label(new Rect(10, 70, 1500, 50), "Current Speed: " + currentSpeed + " MPH", style);
            //GUI.Label(new Rect(10, 70, 1500, 50), "Health: " + health, style);
            //GUI.Label(new Rect(10, 90, 1500, 50), "Damage: " + damage, style);
            //GUI.Label(new Rect(10, 110, 1500, 50), "Immune Status: " + isImmune, style);
            //GUI.Label(new Rect(10, 130, 1500, 50), "Immune Timer: " + immuneTimer, style);
            //GUI.Label(new Rect(10, 150, 1500, 50), "Handbrake Applied: " + handBrake, style);


            //GUI.Label(new Rect(10, 130, 500, 50), "Health: " + health, style);
        }
    }

    void ReloadScene()
    {

    }
}

