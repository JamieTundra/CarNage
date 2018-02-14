using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera Variables
    public float smoothing = 6f;
    public Transform lookAtTarget;
    public Transform positionTarget;
    public Transform sideView;

    // Camera Setup
    public static int totalCameras = 0;
    public static int camerasToSetup = 4;

    // Flag to check if showing sideview
    bool m_ShowingSideView = false;

    // Healthbar Variables
    public GameObject car;
    Canvas healthBar;

    // Flag
    string cameraName;
    string cameraNameToPlayer;
    bool cameraSetup = false;
    bool viewPortsInitialised = false;

    private void Start()
    {
        cameraName = gameObject.name;
        cameraNameToPlayer = cameraName.Substring(0, (cameraName.Length - 6));
        bool doesPlayerExist = DoesPlayerExist();


        if (doesPlayerExist)
        {
            Debug.Log(cameraNameToPlayer + " found, attempting to initialise camera: " + cameraName);
            InitialiseCamera();
        }
        else
        {
            DestroyCamera();
        }
    }

    private void Update()
    {
        if (camerasToSetup == 0 && !viewPortsInitialised)
        {
            Debug.Log("Attempting to setup viewports");
            SetupViewPorts();
        }

        /* Adjust the sideview flag on input
        if (Input.GetKeyDown("v"))
        {
            m_ShowingSideView = !m_ShowingSideView;
        }*/
    }

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (cameraSetup)
        {
            // If we are showing sideview
            if (m_ShowingSideView)
            {
                // Set the cameras position to match the sideview from the CameraRig
                transform.position = sideView.position;
                transform.rotation = sideView.rotation;
                healthBar.transform.rotation = sideView.rotation;
            }
            else
            {
                // 
                transform.position = Vector3.Lerp(transform.position, positionTarget.position, Time.deltaTime * smoothing);
                transform.LookAt(lookAtTarget);
                healthBar.transform.rotation = positionTarget.rotation;
            }
        }

    }

    bool DoesPlayerExist()
    {
        try
        {
            //Debug.Log(cameraNameToPlayer);
            bool attemptToFindPlayer = GameObject.FindGameObjectWithTag(cameraNameToPlayer);
            GameObject aGo = GameObject.FindGameObjectWithTag(cameraNameToPlayer);

            if (attemptToFindPlayer)
            {
                if (aGo != null)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    void InitialiseCamera()
    {
        car = GameObject.FindGameObjectWithTag(cameraNameToPlayer);
        Debug.Log("The camera " + cameraName + " is attempting to attach to the car: " + car.name);
        healthBar = car.GetComponentInChildren<Canvas>();

        lookAtTarget = car.transform.Find("Meshes/CamRig/CamLookAtTarget");
        positionTarget = car.transform.Find("Meshes/CamRig/CamPosition");
        sideView = car.transform.Find("Meshes/CamRig/CamSidePosition");

        Debug.Log(cameraName + (" initialised successfully to: " + cameraNameToPlayer));
        cameraSetup = true;
        ++totalCameras;
        --camerasToSetup;
    }

    void DestroyCamera()
    {
        Debug.Log(cameraNameToPlayer +  " not found, deleting camera: " + cameraName);
        Destroy(gameObject);
        --camerasToSetup;
    }

    void SetupViewPorts()
    {
        switch (totalCameras)
        {
            case 1:
                // setup one
                gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                // setup two
                if (cameraName == "PlayerOneCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
                }
                else
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
                }
                break;
            case 3:
                // setup three
                if (cameraName == "PlayerOneCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                else if (cameraName == "PlayerTwoCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0.25f, 0, 0.5f, 0.5f);
                }
                break;
            case 4:
                // setup four
                if (cameraName == "PlayerOneCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                else if (cameraName == "PlayerTwoCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else if (cameraName == "PlayerThreeCamera")
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                }
                else
                {
                    gameObject.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                }
                break;
            default:
                break;
        }

        viewPortsInitialised = true;
    }
}
