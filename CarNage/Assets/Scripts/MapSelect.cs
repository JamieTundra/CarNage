using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{

    public Button testMapButton;

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
            testMapButton = buttonHolder.transform.Find("TestMap").GetComponent<Button>();

            testMapButton.onClick.AddListener(StoreMap);
        }
    }
}
