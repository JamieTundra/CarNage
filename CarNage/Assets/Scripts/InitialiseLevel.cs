using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialiseLevel : MonoBehaviour {

    PlayerManager playerManager;
    List<GameObject> spawnPoints = new List<GameObject>();

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        Init();
        Debug.Log("Init");
    }

    void Init()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject item in spawns)
        {
            spawnPoints.Add(item);
            //Debug.Log("Add Spawns");
        }

        /*foreach (GameObject item in spawnPoints)
        {
            Debug.Log(item.name);
        }*/


        foreach (Player player in playerManager.players)
        {
            string spawnName = player.m_playerName + "Spawn";
            Debug.Log(player.m_playerName);

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Debug.Log("Spawn Name is: " + spawnPoints[i].name);
                if (spawnPoints[i].name == spawnName)
                {
                    SpawnPlayers(spawnPoints[i], player);
                    Debug.Log("Spawn Name Matches");
                }               
            }        
        }
    }

    void SpawnPlayers(GameObject spawn, Player player)
    {
        GameObject newPlayer = Instantiate(player.m_selectedCar, spawn.transform.position, Quaternion.identity) as GameObject;
        newPlayer.tag = player.m_playerName;
        newPlayer.GetComponent<InputHandler>().m_controllerID = player.m_controllerID;
    }
}
