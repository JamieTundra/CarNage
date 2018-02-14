using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour {

    public List<Transform> m_spawnPoints;


	void Start ()
    {
        
	}

    void SpawnPlayers()
    {
        /*foreach (PlayerData player in PlayerSelect.m_playerList)
        {
            int x = Random.Range(0, m_spawnPoints.Count);
            Instantiate(player.m_chosenCar, m_spawnPoints[x].transform.position, Quaternion.identity);
            m_spawnPoints.RemoveAt(x);
        }*/
    }

}
