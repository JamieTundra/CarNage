using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static int numberOfPlayers;

    void StorePlayerAmount()
    {
        string players = EventSystem.current.currentSelectedGameObject.name;

        switch (players)
        {
            case "SinglePlayer":
                PlayerManager.instance.numberOfPlayers = 1;
                LoadMapSelect();
                break;
            case "2p":
                PlayerManager.instance.numberOfPlayers = 2;
                LoadMapSelect();
                break;
            case "3p":
                PlayerManager.instance.numberOfPlayers = 3;
                LoadMapSelect();
                break;
            case "4p":
                PlayerManager.instance.numberOfPlayers = 4;
                LoadMapSelect();
                break;
            default:
                break;
        }
    }

    void LoadMapSelect()
    {
        SceneManager.LoadScene("MapSelect");
    }
}
