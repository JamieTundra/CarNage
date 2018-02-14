using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour {

    public static GameObject[] carHolder;
    public float rotationSpeed = 60f;
    int carHolderIndex = 0;
    public GameObject currentPreview;
    Vector3 previewPosition;
    public bool debugMode = false;

    void Start()
    {
        // some code that finds out what player we are
        // for now we hard code that we are PlayerOne

        previewPosition = new Vector3(-6.1f, 0.5f, 0f);

        carHolder = Resources.LoadAll<GameObject>("Cars");
        currentPreview = Instantiate(carHolder[carHolderIndex], previewPosition, Quaternion.identity) as GameObject;
        currentPreview.GetComponent<Rigidbody>().isKinematic = true;
        currentPreview.GetComponent<CarController>().enabled = false;
        currentPreview.GetComponentInChildren<Canvas>().enabled = false;
    }
    
    void Update()
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

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //Save selection
            print("Save selection");
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
