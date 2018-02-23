using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{

    public Button debugButton;
    public Button arenaButton;
    public Button trackButton;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StoreMap()
    {
        string mapName = EventSystem.current.currentSelectedGameObject.name;
        PlayerManager.instance.SelectMap("Map_" + mapName);
        LoadCarSelect();
    }

    void LoadCarSelect()
    {
        SceneManager.LoadScene("CarSelect");
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapSelect")
        {
            //Debug.Log("Wanker");
            GameObject buttonHolder = GameObject.Find("ButtonHolder");
            debugButton = buttonHolder.transform.Find("Debug").GetComponent<Button>();
            arenaButton = buttonHolder.transform.Find("Arena").GetComponent<Button>();
            trackButton = buttonHolder.transform.Find("Track").GetComponent<Button>();

            debugButton.onClick.AddListener(StoreMap);
            arenaButton.onClick.AddListener(StoreMap);
            trackButton.onClick.AddListener(StoreMap);
        }
    }
}
