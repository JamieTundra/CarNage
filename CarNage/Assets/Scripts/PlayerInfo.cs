using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInfo {

    public GameObject m_chosenCar;
    public string m_playerTag;
    public string m_controllerID;
    

    public PlayerInfo()
    {

    }

    public void SetData(GameObject chosenCar, string playerTag, string controllerID)
    {
        m_chosenCar = chosenCar;
        m_playerTag = playerTag;
        m_controllerID = controllerID;

    }
}
