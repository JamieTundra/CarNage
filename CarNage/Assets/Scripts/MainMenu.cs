using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static int numberOfPlayers;

    public void LoadPlayerSelect()
    {
        string players = EventSystem.current.currentSelectedGameObject.name;

        switch (players)
        {
            case "SinglePlayer":
                numberOfPlayers = 1;
                break;
            case "2p":
                numberOfPlayers = 2;
                break;
            case "3p":
                numberOfPlayers = 3;
                break;
            case "4p":
                numberOfPlayers = 4;
                break;
            default:
                break;
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<CarSelect>().SetPlayers(numberOfPlayers);
        SceneManager.LoadScene("CarSelect");
    }
}
