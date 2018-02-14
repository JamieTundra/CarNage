using System;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    // Initialisation
    public Transform[] spawnPoints;

    // Car parts
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public Transform centerOfMass;
    public Rigidbody m_rigidBody;
    public GameObject destroyedCar;
    public bool useTireMeshes;

    // Car variables
    public float mass = 500f;
    public float maxSpeed = 200f;
    public float maxTorque = 600f;
    public float turnForce = 60f;
    public float maxBrakeTorque = 30000f;
    //wheelDrive = WheelDrive.FrontWheelDrive;

    //controls
    public bool keyboardEnabled = false;


    // Damage variables
    public Image healthBar;
    public float startingHealth;
    public float health;
    public float damage;
    public float immuneCooldown;
    float immuneTimer;
    bool isImmune;

    // Car movement
    float steer;
    float currentSpeed;
    float accelerate;
    float brake;
    bool handBrake;
    float finalAngle;
    public string controller;
    public bool carInitialised = false;

    // Debugging
    [HideInInspector]
    public GUIStyle style;
    public bool debugMode;
    bool fireDebug = true;

    /*public enum WheelDrive
    {
        FourWheelDrive,
        FrontWheelDrive,
        RearWheelDrive
    }*/

    void FixedUpdate()
    {
        InitialiseCar();
        if (carInitialised)
        {
            if (!keyboardEnabled)
            {
                //Debug.Log("Check inputs");
                steer = Input.GetAxis(controller + "LAnalogX");
                accelerate = Input.GetAxis(controller + "RTrigger");
                brake = -1 * Input.GetAxis(controller + "LTrigger");
                handBrake = Input.GetButton(controller + "XBtn");
                finalAngle = steer * turnForce;
            }
            else if (keyboardEnabled)
            {
                steer = Input.GetAxis(controller + "kLAnalogX");
                accelerate = Input.GetAxis(controller + "kRTrigger");
                brake = -1 * Input.GetAxis(controller + "kLTrigger");
                handBrake = Input.GetButton(controller + "kXBtn");
                finalAngle = steer * turnForce;
            }

            Steer();
            Drive();
            Brake();
            if (useTireMeshes)
            {
                UpdateTirePositions();
            }
        }
        m_rigidBody.mass = mass;

        if (debugMode)
        {
            if (steer != 0) { Debug.Log(controller + " steer" + steer); }
            if (accelerate != 0) { Debug.Log(controller + " accelerate" + accelerate); fireDebug = true; }
            if (brake != 0) { Debug.Log(controller + " brake" + brake); }
            if (handBrake) { Debug.Log(controller + " handBrake" + handBrake); }

            if (fireDebug)
            {
                fireDebug = !fireDebug;
                Debug.Log(maxTorque * accelerate);
                Debug.Log(maxTorque);
            }
        }
    }

    public void InitialiseCar()
    {
        // Initialise the center of mass
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.centerOfMass = centerOfMass.localPosition;

        // Initialise damage variables
        damage = 20f;
        immuneCooldown = 3f;
        isImmune = false;
        startingHealth = 100f;
        health = startingHealth;

        carInitialised = true;
    }

    void Steer()
    {
        wheelColliders[0].steerAngle = finalAngle;
        wheelColliders[1].steerAngle = finalAngle;
    }

    void Drive()
    {

        currentSpeed = 2.23694f * m_rigidBody.velocity.magnitude;

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

        /*switch (wheelDrive)
        {
            case WheelDrive.FourWheelDrive:
                driveTypeMin = 0;
                driveTypeMax = 3;
                break;
            case WheelDrive.FrontWheelDrive:
                driveTypeMin = 0;
                driveTypeMax = 1;
                break;
            case WheelDrive.RearWheelDrive:
                driveTypeMin = 2;
                driveTypeMax = 3;
                break;
            default:
                break;
        }*/
    }

    void Brake()
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

    void UpdateTirePositions()
    {
        // Variables to use within loop
        Quaternion quat;
        Vector3 pos;

        for (int i = 0; i < 4; i++)
        {

            // Get current values of the colliders
            wheelColliders[i].GetWorldPose(out pos, out quat);
            quat.eulerAngles = new Vector3(quat.x, quat.y, 0);
             

            // Set to values to meshes
            tireMeshes[i].position = pos;
            tireMeshes[i].rotation = quat;
        }
    }

    bool CheckImmunity()
    {
        if (isImmune && immuneTimer <= immuneCooldown)
        {
            immuneTimer += Time.deltaTime;
            return true;
        }
        else if (isImmune && immuneTimer > immuneCooldown)
        {
            return false;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("DealsDamage") && !CheckImmunity())
        {
            //Debug.Log("We took damage");
            health -= damage;
            if (health <= 0)
            {
                healthBar.fillAmount = health / startingHealth;
                Death();
                Debug.Log("We died");
            }
            else
            {
                healthBar.fillAmount = health / startingHealth;
                immuneTimer = 0;
                isImmune = true;
            }

        }
        else if (collision.collider.CompareTag("CanBeDamaged"))
        {
            //Debug.Log("We dealt damage");
            immuneTimer = 0;
            isImmune = true;
        }
    }

    private void Death()
    {
        // Disable all colliders to avoid double deaths
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        // Spawn dead car
        Instantiate(destroyedCar, transform.position, transform.rotation);
        // Destroy car
        Destroy(gameObject);  
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
            GUI.Label(new Rect(10, 70, 1500, 50), "Health: " + health, style);
            GUI.Label(new Rect(10, 90, 1500, 50), "Damage: " + damage, style);
            GUI.Label(new Rect(10, 110, 1500, 50), "Immune Status: " + isImmune, style);
            GUI.Label(new Rect(10, 130, 1500, 50), "Immune Timer: " + immuneTimer, style);
            GUI.Label(new Rect(10, 150, 1500, 50), "Handbrake Applied: " + handBrake, style);
            GUI.Label(new Rect(10, 170, 1500, 50), "Current Speed: " + Mathf.Round(currentSpeed) + " MPH", style);
            
            //GUI.Label(new Rect(10, 130, 500, 50), "Health: " + health, style);
        }
    }
}
