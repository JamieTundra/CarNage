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
    float averageRPM;
    float selfRightStartTime;
    float selfRightDelay = 3f;
    bool selfRightTimer = false;
    bool canSelfRight = false;
    public bool selfRighting = false;
    [SerializeField]
    Vector3 currentRotation;

    // Debugging
    [HideInInspector]
    public GUIStyle style;
    public bool debugMode;


    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        rigidBody = GetComponent<Rigidbody>();
        mass = carData.m_mass;
        maxWheelRPM = carData.m_maxWheelRPM;
        maxTorque = carData.m_maxTorque;
        turnForce = carData.m_turnForce;
        maxBrakeTorque = carData.m_maxBrakeTorque;
        rigidBody.centerOfMass = centerOfMass.transform.position;
        rigidBody.mass = mass;

        this.GetComponent<InputHandler>().m_carInit = true;

    }

    private void FixedUpdate()
    {
        currentRotation = this.transform.rotation.eulerAngles;
        CheckSelfRightState();
        SelfRight();
    }


    void CheckSelfRightState()
    {
        if (!selfRightTimer)
        {
            if (averageRPM < 150)
            {
                if (currentRotation.x >= 90 && currentRotation.x <= 270 || currentRotation.z >= 90 && currentRotation.z <= 270)
                {
                    selfRightStartTime = Time.time;
                    selfRightTimer = true;
                }
                else
                {
                    canSelfRight = false;
                }
            }
        }
        else
        {
            if (currentRotation.x >= 90 && currentRotation.x <= 270 || currentRotation.z >= 90 && currentRotation.z <= 270)
            {
                if (selfRightStartTime + selfRightDelay == Time.time)
                {
                    canSelfRight = true;
                }
            }
            else
            {
                selfRightTimer = false;
                canSelfRight = false;
            }
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
        if (canSelfRight && !selfRighting)
        {
            Debug.Log("Attempting to self right");
            selfRighting = true;
            this.transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentRotation), Quaternion.Euler(0, currentRotation.y, 0), 600f);
            selfRighting = false;
            selfRightTimer = false;

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

