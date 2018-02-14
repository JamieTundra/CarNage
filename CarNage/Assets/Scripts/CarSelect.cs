using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelect : MonoBehaviour {

    #region Singleton
    public static CarSelect instance;
    public int numberOfPlayers;
    public int playersReady = 0;

    private void Awake()
    {
        numberOfPlayers = PlayerManager.instance.numberOfPlayers;
        SetupLayout(numberOfPlayers);
    }

    private void SetupLayout(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player p = new Player
            {
                playerName = ("Player" + i),
                carID = ("Unassigned"),
                tireID = ("Unassigned"),
                controllerID = (i)
            };

            PlayerManager.instance.AddPlayer(p);
        }
    }
    #endregion
}
