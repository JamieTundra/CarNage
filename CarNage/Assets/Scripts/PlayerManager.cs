using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singelton
    public static PlayerManager instance;
    public int numberOfPlayers;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    #endregion

    #region Maps
    [SerializeField]
    public string currentMapName;

    public void SelectMap(string mapName)
    {
        if (mapName == null)
            return; // bad input

        currentMapName = mapName; // set current map
    }
    #endregion

    #region Player
    [SerializeField]
    public List<Player> Players = new List<Player>();

    public void AddPlayer(Player p)
    {
        Players.Add(p); // add new player
    }
    #endregion
}

[System.Serializable]
public class Player
{
    public string playerName;
    public int controllerID;
    public string carID;
    public string tireID;
    public bool playerReady;
}


