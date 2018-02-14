using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapSelect : MonoBehaviour {

    public void StoreMap()
    {
        string mapName = EventSystem.current.currentSelectedGameObject.name;
        PlayerManager.instance.SelectMap("Map_" + mapName);

    }

    void LoadCarSelect()
    {
        SceneManager.LoadScene("CarSelect");
    }
}
