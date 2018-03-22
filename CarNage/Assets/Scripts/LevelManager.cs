using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    #region Singleton
    public LevelManager instance;
    //public List<Transform> spawnPoints = new List<Transform>();
    //public GameObject spawnHolder;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //InitialiseSpawnPoints();

        foreach (Player p in PlayerManager.instance.Players)
        {
            SpawnPlayer(p);
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("StartButton"))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    /*void InitialiseSpawnPoints()
    {
        spawnHolder = GameObject.Find("SpawnPointHolder");
        for (int i = 0; i < spawnHolder.transform.childCount; i++)
        {
            spawnPoints.Add(spawnHolder.transform.GetChild(i));
        }
    }*/

    void SpawnPlayer(Player p)
    {
        //Transform chosenSpawn = spawnPoints[p.controllerID - 1];
        //Debug.Log(chosenSpawn.position);
        string carName = p.carID.Substring(0, (p.carID.Length - 7));
        GameObject chosenCar = Resources.Load("Cars/" + carName) as GameObject;
        GameObject player = Instantiate(chosenCar, new Vector3(0, 0, 0), Quaternion.Euler(0, -90, 0));
        player.tag = p.playerName;
        player.transform.SetParent((GameObject.Find(p.playerName + "Holder").transform));
    }

    #endregion
}
