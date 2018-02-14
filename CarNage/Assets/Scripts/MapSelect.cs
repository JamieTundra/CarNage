using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapSelect : MonoBehaviour {

    /* Debugging
    private void Update()
    {
        mapName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(mapName);
    }*/

    public void LoadMap()
    {
        string mapName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Attempting to load: Map_" + mapName);
        //SceneManager.LoadScene("Map_" + mapName);
    }
}
