using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    #region Singleton
    public static CameraManager instance;
    public Rect[] cameraPositions;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        SetupCameraPositions();
        SetupCameras(PlayerManager.instance.numberOfPlayers);
    }

    private void SetupCameraPositions()
    {
        cameraPositions = new Rect[10];

        cameraPositions[0] = new Rect(0, 0, 1, 1); // P1/1
        cameraPositions[1] = new Rect(0, 0, 0.5f, 1); // P1/2
        cameraPositions[2] = new Rect(0.5f, 0, 0.5f, 1); // P2/2
        cameraPositions[3] = new Rect(0, 0.5f, 0.5f, 0.5f); // P1/3
        cameraPositions[4] = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // P2/3
        cameraPositions[5] = new Rect(0.25f, 0, 0.5f, 0.5f); // P3/3
        cameraPositions[6] = new Rect(0, 0.5f, 0.5f, 0.5f); // P1/4
        cameraPositions[7] = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // P2/4
        cameraPositions[8] = new Rect(0, 0, 0.5f, 0.5f); // P3/4
        cameraPositions[9] = new Rect(0.5f, 0, 0.5f, 0.5f); // P4/4
    }

    private void SetupCameras(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            CameraData c = new CameraData
            {
                cameraName = ("Player" + (i + 1) + "Camera"),
                cameraPlayer = i +1,
                cameraCar = GameObject.FindGameObjectWithTag("Player" + (i + 1)),
            };
            CameraManager.instance.AddCamera(c);
        }

        SetupCameraLayout(numberOfPlayers);
    }

    private void SetupCameraLayout(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 1:
                InitialiseCamera(CameraManager.instance.Cameras[0], cameraPositions[0]);
                break;
            case 2:
                InitialiseCamera(CameraManager.instance.Cameras[0], cameraPositions[1]);
                InitialiseCamera(CameraManager.instance.Cameras[1], cameraPositions[2]);
                break;
            case 3:
                InitialiseCamera(CameraManager.instance.Cameras[0], cameraPositions[3]);
                InitialiseCamera(CameraManager.instance.Cameras[1], cameraPositions[4]);
                InitialiseCamera(CameraManager.instance.Cameras[2], cameraPositions[5]);
                break;
            case 4:
                InitialiseCamera(CameraManager.instance.Cameras[0], cameraPositions[6]);
                InitialiseCamera(CameraManager.instance.Cameras[1], cameraPositions[7]);
                InitialiseCamera(CameraManager.instance.Cameras[2], cameraPositions[8]);
                InitialiseCamera(CameraManager.instance.Cameras[3], cameraPositions[9]);
                break;
            default:
                break;
        }
    }


    private void InitialiseCamera(CameraData camera, Rect rect)
    {
        GameObject currentCamera = Instantiate(Resources.Load("Cameras/CameraPrefab"), Vector3.zero, Quaternion.identity) as GameObject;
        currentCamera.transform.SetParent(GameObject.Find("CameraHolder").transform);
        currentCamera.GetComponent<Camera>().rect = rect;
        CameraController controller = currentCamera.AddComponent<CameraController>();
        controller.lookAtTarget = camera.cameraCar.transform.Find("Meshes/CamRig/CamLookAtTarget");
        controller.positionTarget = camera.cameraCar.transform.Find("Meshes/CamRig/CamPosition");
    }

    #region Cameras
    [SerializeField]
    public List<CameraData> Cameras = new List<CameraData>();

    public void AddCamera(CameraData c)
    {
        Cameras.Add(c); // add new camera
    }
    #endregion

    [System.Serializable]
    public class CameraData
    {
        public string cameraName;
        public GameObject cameraCar;
        public float cameraPlayer;
    }
    #endregion
}
