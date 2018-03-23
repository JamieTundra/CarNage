using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    // Car parts
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Rigidbody rigidBody;


    // Car variables
    public Car carData;
    public float mass;
    public float maxWheelRPM;
    public float maxSpeed;
    public float currentSpeed;
    public float turnAngle;
    public float turnSpeed;
    public float torque;
    public float brakeTorque;
    public Transform centerOfMass;
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
        mass = carData.m_mass;
        rigidBody.mass = mass;
        rigidBody.centerOfMass = centerOfMass.localPosition;
    }

    public void InitValues()
    {

        maxWheelRPM = carData.m_maxWheelRPM;
        maxSpeed = carData.m_maxSpeed;
        turnAngle = carData.m_turnAngle;
        turnSpeed = carData.m_turnSpeed;
        torque = carData.m_torque;
        brakeTorque = carData.m_BrakeTorque;


        this.GetComponent<InputHandler>().m_carInit = true;
    }

    private void Update()
    {
        GroundCheck();
        StartSelfRight();
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
        float finalAngle = steer * turnAngle;
        wheelColliders[0].steerAngle = Mathf.Lerp(wheelColliders[0].steerAngle, finalAngle, Time.deltaTime * turnSpeed);
        wheelColliders[1].steerAngle = Mathf.Lerp(wheelColliders[1].steerAngle, finalAngle, Time.deltaTime * turnSpeed);
    }

    public void Drive(float drivingForce)
    {
        currentSpeed = Mathf.Round(2 * Mathf.PI * wheelColliders[0].radius * wheelColliders[0].rpm * 60 / 1000);

        if (currentSpeed < maxSpeed)
        {
            if (drivingForce > 0.05f)
            {
                wheelColliders[0].motorTorque = torque;
                wheelColliders[1].motorTorque = torque;
            }
            else if (drivingForce < -0.05f)
            {
                wheelColliders[0].motorTorque = -torque;
                wheelColliders[1].motorTorque = -torque;
            }
        }
        else
        {
            wheelColliders[0].motorTorque = 0;
            wheelColliders[1].motorTorque = 0;
        }
    }

    public void HandBrake(bool handBrake)
    {
        if (handBrake)
        {
            wheelColliders[0].brakeTorque = brakeTorque;
            wheelColliders[1].brakeTorque = brakeTorque;
            wheelColliders[2].brakeTorque = Mathf.Infinity;
            wheelColliders[3].brakeTorque = Mathf.Infinity;
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

