using System.Collections;
using UnityEngine;

public class CarPreview : MonoBehaviour
{

    public static GameObject[] carHolder;
    public GameObject previewHolder;
    public GameObject previewMask;
    public float rotationSpeed = 60f;
    int carHolderIndex = 0;
    public GameObject currentPreview;
    Vector3 previewPosition;
    public bool debugMode = false;
    [SerializeField]
    Player player;
    bool delayTimerOn = false;

    private void Start()
    {
        carHolder = Resources.LoadAll<GameObject>("Cars");
        previewHolder = this.gameObject;
        GameObject canvas = GameObject.Find("Canvas");
        int playerID = int.Parse(previewHolder.name.Substring(6, 1)) - 1;
        foreach (Transform preview in canvas.transform)
        {
            if (preview.name == "Player" + (playerID + 1) + "PreviewMask")
            {
                previewMask = preview.gameObject;
            }
        }
        player = PlayerManager.instance.Players[playerID];

    }

    void Update()
    {
        if (player.playerReady == true)
        {
            if (Input.GetButtonUp(player.controllerID + "AButton") || Input.GetButtonUp(player.controllerID + "BButton"))
            {
                StopAllCoroutines();
                previewMask.SetActive(false);
                delayTimerOn = false;
                player.playerReady = false;
            }
        }
        else
        {
            currentPreview = transform.GetChild(0).gameObject;
            currentPreview.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            previewPosition = currentPreview.transform.position;

            if (Input.GetButtonUp(player.controllerID + "AButton"))
            {
                StopAllCoroutines();
                delayTimerOn = false;
                currentPreview.transform.eulerAngles = new Vector3(0f, -150f, 0f);
                previewMask.SetActive(true);
                player.playerReady = true;
            }

            if (Input.GetAxis(player.controllerID + "LAnalogX") > 0 || Input.GetAxis(player.controllerID + "DPadX") > 0)
            {
                if (!delayTimerOn)
                {
                    StartCoroutine(PreviewDelay());
                    UpdatePreview(1, previewPosition);
                }
            }

            if (Input.GetAxis(player.controllerID + "LAnalogX") < 0 || Input.GetAxis(player.controllerID + "DPadX") < 0)
            {
                if (!delayTimerOn)
                {
                    StartCoroutine(PreviewDelay());
                    UpdatePreview(-1, previewPosition);
                }
            }
        }
    }


    IEnumerator PreviewDelay()
    {
        delayTimerOn = true;
        yield return new WaitForSeconds(0.5f);
        delayTimerOn = false;
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

        currentPreview = Instantiate(carHolder[carHolderIndex], previewPosition, Quaternion.Euler(spawnRotation)) as GameObject;
        currentPreview.transform.SetParent(GameObject.Find(player.playerName + "Preview").transform);
        currentPreview.GetComponent<Rigidbody>().isKinematic = true;
        currentPreview.GetComponent<CarController>().enabled = false;
        currentPreview.GetComponent<InputHandler>().enabled = false;
        currentPreview.GetComponentInChildren<Canvas>().enabled = false;
        player.carID = currentPreview.name;
    }

    void OnGUI()
    {
        if (debugMode)
        {
            GUI.Label(new Rect(10, 10, 1500, 50), carHolderIndex.ToString());
        }

    }
}
