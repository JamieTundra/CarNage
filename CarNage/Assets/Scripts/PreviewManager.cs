using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewManager : MonoBehaviour
{

    #region Singleton
    public static PreviewManager instance;
    public int playersReady = 0;
    public Vector3[] previewPoints = new Vector3[10];
    float countdownValue = 1f;
    float currCountdownValue;
    public TextMeshProUGUI countdownText;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        SetupPreviewPoints();
        SetupPlayers(PlayerManager.instance.numberOfPlayers);
    }

    private void Update()
    {
        if (playersReady == PlayerManager.instance.numberOfPlayers)
        {
            StartCoroutine(StartCountdown(countdownValue));
        }
        else
        {
            StopAllCoroutines();
            countdownText.gameObject.SetActive(false);
        }
    }

    private void SetupPreviewPoints()
    {
        previewPoints[0] = new Vector3(0f, -0.5f, 0f); // P1/1
        previewPoints[1] = new Vector3(-4.45f, -0.5f, 0f); // P1/2
        previewPoints[2] = new Vector3(4.45f, -0.5f, 0f); // P2/2
        previewPoints[3] = new Vector3(-4.45f, 2f, 0f); // P1/3
        previewPoints[4] = new Vector3(4.45f, 2f, 0f); // P2/3
        previewPoints[5] = new Vector3(0f, -3f, 0f); // P3/3
        previewPoints[6] = new Vector3(-4.45f, 2f, 0f); // P1/4
        previewPoints[7] = new Vector3(4.45f, 2f, 0f); // P2/4
        previewPoints[8] = new Vector3(-4.45f, -3f, 0f); // P3/4
        previewPoints[9] = new Vector3(4.45f, -3f, 0f); // P4/4
    }

    public void SetupPlayers(int numberOfPlayers)
    {
        //Debug.Log("Cock");
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player p = new Player
            {
                playerName = ("Player" + (i + 1)),
                carID = ("Unassigned"),
                tireID = ("Unassigned"),
                playerReady = false,
                controllerID = i + 1,
            };
            PlayerManager.instance.AddPlayer(p);
        }

        SetupLayout(numberOfPlayers);
    }
    private void SetupLayout(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 1:
                GeneratePreview(PlayerManager.instance.Players[0], previewPoints[0]);
                break;
            case 2:
                GeneratePreview(PlayerManager.instance.Players[0], previewPoints[1]);
                GeneratePreview(PlayerManager.instance.Players[1], previewPoints[2]);
                break;
            case 3:
                GeneratePreview(PlayerManager.instance.Players[0], previewPoints[3]);
                GeneratePreview(PlayerManager.instance.Players[1], previewPoints[4]);
                GeneratePreview(PlayerManager.instance.Players[2], previewPoints[5]);
                break;
            case 4:
                GeneratePreview(PlayerManager.instance.Players[0], previewPoints[6]);
                GeneratePreview(PlayerManager.instance.Players[1], previewPoints[7]);
                GeneratePreview(PlayerManager.instance.Players[2], previewPoints[8]);
                GeneratePreview(PlayerManager.instance.Players[3], previewPoints[9]);
                break;
            default:
                break;
        }
    }

    public void GeneratePreview(Player p, Vector3 previewPoint)
    {
        GameObject[] carHolder = Resources.LoadAll<GameObject>("Cars");

        GameObject currentPreview = Instantiate(carHolder[0], new Vector3(0f, -0.5f, 0f), Quaternion.identity) as GameObject;
        currentPreview.transform.SetParent(GameObject.Find(p.playerName + "Preview").transform);
        currentPreview.transform.parent.gameObject.AddComponent<CarPreview>();
        currentPreview.transform.parent.gameObject.transform.position = previewPoint;
        currentPreview.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        currentPreview.GetComponent<Rigidbody>().isKinematic = true;
        currentPreview.GetComponent<CarController>().enabled = false;
        currentPreview.GetComponent<InputHandler>().enabled = false;
        p.carID = currentPreview.name;
    }

    IEnumerator StartCountdown(float countDownFrom)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue >= 0)
        {
            countdownText.text = ("STARTING IN: " + currCountdownValue + " SECONDS");
            countdownText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }

        LoadMap(PlayerManager.instance.currentMapName);
    }

    private void LoadMap(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    #endregion
}
