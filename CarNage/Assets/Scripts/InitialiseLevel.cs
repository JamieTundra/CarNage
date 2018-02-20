using System.Collections.Generic;
using UnityEngine;

public class InitialiseLevel : MonoBehaviour
{

    List<GameObject> spawnPoints = new List<GameObject>();

    void Start()
    {
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


        foreach (Player player in PlayerManager.instance.Players)
        {
            string spawnName = player.playerName + "Spawn";
            Debug.Log(player.playerName);

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
        GameObject chosenCar = Resources.Load("Cars/" + player.carID) as GameObject;

        GameObject newPlayer = Instantiate(chosenCar, spawn.transform.position, Quaternion.identity) as GameObject;
        newPlayer.tag = player.playerName;
        newPlayer.GetComponent<InputHandler>().m_controllerID = player.controllerID;
    }
}
