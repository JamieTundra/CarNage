using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public List<Player> players = new List<Player>();

    public void CreatePlayer(string playerName, int controllerID, GameObject selectedCar)
    {
        Player player = new Player
        {
            m_playerName = playerName,
            m_controllerID = controllerID,
            m_selectedCar = selectedCar
        };

        players.Add(player);

        foreach (Player item in players)
        {
            Debug.Log(player.m_selectedCar.name);
        }
    }
}
