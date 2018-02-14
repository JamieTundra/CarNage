using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelect : MonoBehaviour {

    public static int m_numberOfPlayers;
    public static GameObject[] carHolder;
    public float rotationSpeed = 60f;
    int carHolderIndex = 0;
    public GameObject currentPreview;
    public int controllerID;
    public string playerName;
    Vector3 previewPosition;
    public bool debugMode = false;
    bool layoutSetup = false;
    int playersReady = 0;

    private void Start()
    {
        SetupLayout();
    }

    public void SetPlayers(int numberOfPlayers)
    {
        m_numberOfPlayers = numberOfPlayers;
    }

    public void SetupLayout()
    {
        // Code that sets up layout
        // some code that finds out what player we are
        // for now we hard code that we are PlayerOne
        playerName = "PlayerOne";
        controllerID = 1;

        previewPosition = new Vector3(-6.1f, 0.5f, 0f);

        carHolder = Resources.LoadAll<GameObject>("Cars");
        currentPreview = Instantiate(carHolder[carHolderIndex], previewPosition, Quaternion.identity) as GameObject;
        currentPreview.GetComponent<Rigidbody>().isKinematic = true;
        currentPreview.GetComponent<CarController>().enabled = false;
        currentPreview.GetComponent<InputHandler>().enabled = false;
        currentPreview.GetComponentInChildren<Canvas>().enabled = false;

        layoutSetup = true;
        Debug.Log(m_numberOfPlayers);
    }
    
    void Update()
    {
        if (layoutSetup)
        {
            currentPreview.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UpdatePreview(1, previewPosition);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UpdatePreview(-1, previewPosition);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                FindObjectOfType<PlayerManager>().CreatePlayer(playerName, controllerID, currentPreview);
                playersReady++;
            }
        }

        if (m_numberOfPlayers != 0)
        {
            if (playersReady == m_numberOfPlayers)
            {
                SceneManager.LoadScene("MapSelect");
            }
        }

    }

    void UpdatePreview(int direction, Vector3 previewPosition)
    {
        Vector3 spawnRotation = currentPreview.transform.rotation.eulerAngles;
        Destroy(currentPreview);

        carHolderIndex += direction;
        if (carHolderIndex == -1)
        {
            carHolderIndex = carHolder.Length - 1;
            //Debug.Log("Array went below 0, so we set the new value to the maximum of the array");
            //Debug.Log(carHolderIndex);
        }
        else if (carHolderIndex > carHolder.Length - 1)
        {
            carHolderIndex = 0;
            //Debug.Log("Array went above length, so we set the new value to the start of the array");
            //Debug.Log(carHolderIndex);
        }

        currentPreview = Instantiate(carHolder[carHolderIndex], previewPosition, Quaternion.Euler(spawnRotation));
        currentPreview.GetComponent<Rigidbody>().isKinematic = true;
        currentPreview.GetComponent<CarController>().enabled = false;
        currentPreview.GetComponent<InputHandler>().enabled = false;
        currentPreview.GetComponentInChildren<Canvas>().enabled = false;
    }

    void OnGUI()
    {
        if (debugMode)
        {
            GUI.Label(new Rect(10, 10, 1500, 50), carHolderIndex.ToString());
        }
        
    }
}
