using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CarController : MonoBehaviour
{
    // Car parts
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Rigidbody rigidBody;


    // Car variables
    public Car carData;
    public float mass;
    public float maxSpeed;
    public float currentSpeed;
    public float mphSpeed;
    public float turnAngle;
    public float turnSpeed;
    public float torque;
    public float brakeTorque;
    public Transform centerOfMass;
    public bool isOnGround;

    // Steps
    public float criticalSpeed;
    public int stepsBelow;
    public int stepsAbove;

    public bool canSelfRight = true;
    public bool selfRighting = false;
    public bool clearOfGround = false;
    Vector3 currentPosition;
    Vector3 hoverPosition;
    Vector3 currentRotation;
    Vector3 targetRotation;
    float rotationTime;
    float moveTime;




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
        maxSpeed = carData.m_maxSpeed;
        turnAngle = carData.m_turnAngle;
        turnSpeed = carData.m_turnSpeed;
        torque = carData.m_torque;
        brakeTorque = carData.m_BrakeTorque;
        criticalSpeed = carData.m_criticalSpeed;
        stepsBelow = carData.m_stepsBelow;
        stepsAbove = carData.m_stepsAbove;
        mass = carData.m_mass;

        rigidBody.mass = mass;
        rigidBody.centerOfMass = centerOfMass.localPosition;

        this.GetComponent<InputHandler>().m_carInit = true;
    }

    private void Update()
    {
        GroundCheck();
        StartSelfRight();
        InitValues();
        wheelColliders[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
        currentSpeed = rigidBody.velocity.magnitude;
        mphSpeed = 2.23694f * rigidBody.velocity.magnitude;
    }

    private void StartSelfRight()
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
    }

    private void GroundCheck()
    {
        Debug.DrawRay(this.transform.position, Vector3.up * 2f, Color.red);
        Debug.DrawRay(this.transform.position, Vector3.down * 2f, Color.red);

        if (Physics.Raycast(this.transform.position, Vector3.up, 2f) || (Physics.Raycast(this.transform.position, Vector3.down, 2f)))
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
        float finalAngle = steer * turnAngle;
        wheelColliders[0].steerAngle = Mathf.Lerp(wheelColliders[0].steerAngle, finalAngle, Time.deltaTime * turnSpeed);
        wheelColliders[1].steerAngle = Mathf.Lerp(wheelColliders[1].steerAngle, finalAngle, Time.deltaTime * turnSpeed);
    }

    public void Drive(float drivingForce)
    {
    
        if (currentSpeed < maxSpeed)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = torque * drivingForce;
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = 0;
            }
        }
    }

    public void HandBrake(bool handBrake)
    {
        if (handBrake)
        {
            wheelColliders[2].brakeTorque = brakeTorque;
            wheelColliders[3].brakeTorque = brakeTorque;
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
        if (canSelfRight && mphSpeed < 5 && isOnGround)
        {
            //Debug.Log("Attempting to self right");
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
}

