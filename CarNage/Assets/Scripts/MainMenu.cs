using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{

    ////public Button singlePlayerButton;
    ////public Button twoPlayerButton;
    ////public Button threePlayerButton;
    ////public Button fourPlayerButton;
    ////public Button quitButton;
    ////public Button[] buttons = new Button[4];
    public static int numberOfPlayers;

    //private void Start()
    //{
    //    GameObject buttonHolder = GameObject.Find("ButtonHolder");
    //    singlePlayerButton = buttonHolder.transform.Find("SinglePlayer").GetComponent<Button>();
    //    twoPlayerButton = buttonHolder.transform.Find("2p").GetComponent<Button>();
    //    threePlayerButton = buttonHolder.transform.Find("3p").GetComponent<Button>();
    //    fourPlayerButton = buttonHolder.transform.Find("4p").GetComponent<Button>();
    //    quitButton = buttonHolder.transform.Find("Quit").GetComponent<Button>();
    //    buttons[0] = singlePlayerButton;
    //    buttons[1] = twoPlayerButton;
    //    buttons[2] = threePlayerButton;
    //    buttons[3] = fourPlayerButton;

    //}

    //private void Update()
    //{
    //    foreach (string controller in Input.GetJoystickNames())
    //    {
    //        listOfControllers.Add(controller);
    //    }

    //    int amountOfControllers = listOfControllers.Count;
    //    Debug.Log("There are: " + amountOfControllers + " plugged in.");

    //    for (int i = 0; i < amountOfControllers; i++)
    //    {
    //        buttons[i].interactable = true;
    //    }
    //}


    public void StorePlayerAmount()
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
